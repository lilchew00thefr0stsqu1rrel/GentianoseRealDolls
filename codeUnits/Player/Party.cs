using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public class Party : MonoBehaviour
    {

        // Регионы скрипта:
        // 1. Данные
        // 2. Корутины (других корутин не должно быть пред лицем этих корутин)
        // 3. Инициализация скриптов, связанных с SQL
        // 4. Инициализация куклы
        // 5. Методы отряда как системы кукол
        // 6. События Unity

        // TODO: сделать поля открытыми для UI
        // звери больше не зависят от UI, требования принципа DI
        // Каждые 0.1 с обновляется HUD

        // 1. Данные


        // Делегат для события изменения здоровья
        public delegate void ActiveDollChanged(int dollIndexInParty);
        public event ActiveDollChanged OnActiveDollChanged;


        ////// public IObjectResolver _objectResolver;

        [Header("Services")]
        [SerializeField] private int m_MapID;
        [SerializeField] private CurrentSceneData m_CurrentScene;
        public CurrentSceneData CurrentScene => m_CurrentScene;
        [SerializeField] private TimePastStats m_TimePastStats;
        [SerializeField] private TeleportBeasts m_TeleportBeasts;

        [SerializeField] private CameraAroundDoll m_Camera;
        public CameraAroundDoll Camera => m_Camera;
        [SerializeField] private Camera m_ProperCamera;
        [SerializeField] private GaitInputController m_GaitInputController;
        [SerializeField] private MoveInputController m_ShipInputController;


        [Header("UI")]

        [SerializeField] private CombatDashboard m_CombatDashboard;
        [SerializeField] private HabitatInterface m_HabitatInterface;


        [Header("Dependencies")]

        [SerializeField] private DollBase m_SimplePosDB;
        [SerializeField] private AllDollPetStats m_AllDollCharacters;
        [SerializeField] private ActiveDollPosition m_ActiveDollPosition;
        [SerializeField] private AllDollSleeps m_AllDollSleeps;
        [SerializeField] private AllDollCombatStats m_AllDollBattle;
        
        [SerializeField] private ActiveDollUponExit m_ActiveDollUponExit;

        [SerializeField] private StatusOfDolls m_StatusOfDolls;
        public StatusOfDolls StataOfDolls => m_StatusOfDolls;

        [SerializeField] private PoopStore m_PoopStore;


        [SerializeField] private Inventory m_Inventory;
        [SerializeField] private OffField m_OffField;


        [Header("Session")]

        [SerializeField] private bool m_NotFirstSession;
        [SerializeField] private bool m_NotSessionStart;
        [SerializeField] private bool m_NotFirstDoll;

        [Header("Time")]


        [SerializeField] private float m_TickLength = 0.2f;


        [SerializeField] private UnityEngine.UI.Text m_Warn;
        [SerializeField] private SkillStates m_SkillStates;

        private int m_ActiveDollID = 0;

        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }

        [Header("Dolls at the moment")]


        [SerializeField] private Doll[] m_DollPrefabs;
        [SerializeField] private int[] m_DollsCode;

        [SerializeField] private GameObject m_Visual;
        [SerializeField] float dH = 0.7f;


        [SerializeField] private DollDataLists m_DollDataLists;
        public DollDataLists DollData => m_DollDataLists;

        private int m_NumberOfDolls = 4;

        /// <summary>
        /// С 3 VI 2026 нужно внедрить новую систему управления куклами:
        /// Как игровой объект существуют 1 главная кукла и до 2 следующих кукол:
        /// Lead doll и trail dolls
        /// Если скорости совпадают, можно вступить в режим "цуг/караван"
        /// Смена куклы заменяет префаб куклы
        /// Куклы, следующие за главной куклой, функционируют как боты и нпс,
        /// Их поворот копирует поворот главной куклы
        /// Их расстояние фиксировано относительно главной куклы и они находятся чётко за 1 (средняя кукла) 
        /// и 2 (задняя кукла) метра от таза главной куклы
        /// Во время каравана нельзя сменить куклу
        /// </summary>



        [SerializeField] private int[] m_GaitMap;
        public int[] GaitMap => m_GaitMap;
        public void SetGaitMap(int[] gm)
        {
            m_GaitMap = gm;
        }

        [SerializeField] private Doll m_ActiveDoll;
        public Doll ActiveDoll => m_ActiveDoll;

        [SerializeField] private Doll[] m_TrailDolls;
        public Doll[] TrailDolls => TrailDolls;

        private bool m_ProcessionMode;

        [Range(0.0f, 37.0f)]
        [SerializeField] private int m_Stamina;
        public int Stamina => m_Stamina;

        private DollController m_ActiveDollController;
        public DollController ActiveDollController => m_ActiveDollController;


        [SerializeField] private DollAsset[] m_DollAssets;

        // состав
        [SerializeField] private CurrentPartyDolls m_CurrentPartyDolls;

        private int m_ActiveDollIndexInParty;
        public int ActiveDollIndexInParty => m_ActiveDollIndexInParty;
        public bool AreThereSleepingBeasts => m_ActiveDoll.DollController.SleepSystem.IsSleeping;

        public int ActiveDollID => m_ActiveDollID;

        private long m_TimeDifference;

        public static event Action OnChangeActiveDoll;




        // 2. Корутины (других корутин не должно быть пред лицем этих корутин)
       
        // 0.2 c
        IEnumerator UniTick()
        {
            ChangeStamina(1);

            if (m_ActiveDoll.DollController.PositionManager != null)
            {
                m_ActiveDoll.DollController.PositionManager.Save();
            }

            m_AllDollBattle.WriteDoll(new int[2] {m_ActiveDollID, m_ActiveDoll.PetAsSpaceShip.HitPoints});

            yield return new WaitForSeconds(m_TickLength);

            StartCoroutine(UniTick());
                     
        }

        // Граница минут
        IEnumerator UniTickMinute()
        {
            yield return new WaitUntil(() => DateTime.Now.Second == 59);
            yield return new WaitForSeconds(1);

            if (m_ActiveDoll != null)
            {
                List<int> slp = m_AllDollSleeps.GetDolls();
                m_AllDollCharacters.ReduceNonSleepStats();
                m_AllDollCharacters.ChangeSleepStat(slp);

                var d = m_AllDollCharacters.GetDoll(m_ActiveDollID);
                if (d.Length > 0)
                    m_ActiveDoll.FillStats(d);

                m_ActiveDoll.DollController.GoToBed(m_AllDollSleeps.GetDoll(m_ActiveDollID));
            }



            StartCoroutine(UniTickMinute());
            
        }


        // 3. Инициализация скриптов, связанных с SQL


        // Прочесть данные из реляционной БД




        public void InitBase(int mapID, long time)
        {

            m_Inventory.InitInventory();

            m_TimeDifference = m_TimePastStats.ReadTime();

            m_AllDollCharacters.ReadDolls();
            m_AllDollSleeps.ReadDolls();

            m_PoopStore.InitPoop();

            m_ActiveDollIndexInParty = m_ActiveDollUponExit.GetActiveDoll();

            m_NotFirstDoll = false;

            if (!m_NotSessionStart)
            {
                ChangeStatsByPastTime();
                m_NotSessionStart = true;
            }

            
            InitDoll(m_ActiveDollIndexInParty);

            if (!m_NotFirstSession)
            {
                StartCoroutine(UniTick());
                StartCoroutine(UniTickMinute());


                m_NotFirstSession = true;

            }





            print("Pets are ready");
        }





        // 4. Инициализация куклы

        private void ChangeStatsByPastTime()
        {
            int timeI = (int)m_TimeDifference;

            for (int i = 0; i < m_NumberOfDolls; i++)
            {
                int[] stats = new int[8];
                stats = m_AllDollCharacters.GetDoll(i);

                stats[1] = Mathf.Clamp(stats[1] - timeI, 0, 10);

                stats[2] = Mathf.Clamp(stats[2] + timeI, 0, m_DollDataLists.AnalGlandVolumeArray[i]);

                stats[3] = Mathf.Clamp(stats[3] - timeI, 0, 10);

                stats[4] = Mathf.Clamp(stats[4] - timeI, 0, 40);

                stats[5] = Mathf.Clamp(stats[5] - timeI, 0, 30);

                stats[6] = Mathf.Clamp(stats[6] - timeI, 0, 100);

                if (m_AllDollSleeps.GetDolls()[i] == 1)
                   stats[7] = Mathf.Clamp(stats[7] + timeI, 0, 100);
                if (m_AllDollSleeps.GetDolls()[i] == 0)
                   stats[7] = Mathf.Clamp(stats[7] - timeI, 0, 100);

                m_AllDollCharacters.WriteDoll(stats);
            }
        }

        private async void Warn(string text)
        {
            m_Warn.enabled = true;
            m_Warn.text = text;
            await Task.Delay(700);
            m_Warn.enabled = false;
        }

        [SerializeField] private bool m_IsSwimming;

        public void SetSwimming(bool swimming)
        {
            m_IsSwimming = swimming;
        }

        // Создать и заполнить куклу
        // Интерфейс пользователя зависит от зверей, а не наоборот

        public void ChangeDoll(int index)
        {
            if (m_IsSwimming)
            {
                Warn("Невозможно сменить куклу во время плавания");
                return;
            }
            else
            {
                InitDoll(index);
            }
        }

        // Переключиться на данного персонажа
        // Сделать персонажей активным/ неактивными
        public void InitDoll(int index)
        {
            

            m_ActiveDollIndexInParty = index;


            if (m_ActiveDoll) DestroyImmediate(m_ActiveDoll.gameObject);


            // 29 vii 26
            m_DollsCode = m_CurrentPartyDolls.DollConsist;
            
            // 26 vii 26

            m_ActiveDollID = m_DollsCode[index];

            m_ActiveDoll = Instantiate(m_DollPrefabs[m_ActiveDollID], m_ActiveDollPosition.GetDollPos(), Quaternion.identity);





            m_ActiveDoll.DollController.SetDollAsActive(true);



            m_ActiveDollController = m_ActiveDoll.DollController;

            m_ActiveDoll.SetBase(m_AllDollCharacters);
            m_ActiveDoll.DollController.SleepSystem.SetBase(m_AllDollSleeps);
            m_ActiveDoll.DollController.PositionManager.SetBase(m_ActiveDollPosition, m_AllDollSleeps);
            if (m_CurrentScene.GameMode == Mode.Habitat && m_AllDollSleeps.GetDoll(m_ActiveDollID) == true
                
                & m_ActiveDoll != null)
            {
                m_ActiveDoll.DollController.PositionManager.PutIntoBed();
            }

            m_ActiveDollUponExit.SetActiveDoll(index);

            m_ActiveDoll.DollController.SetLocationIndex(m_MapID);
            m_ActiveDoll.DollController.ConstructDollParty(this);
            m_ActiveDoll.DollController.SetOffField(m_OffField);
            m_ActiveDoll.DollController.ConstructInventory(m_Inventory);
            m_ActiveDoll.DollController.ConstructPoop(m_PoopStore);
            m_ActiveDoll.DollController.SetDollProperties();

            m_ActiveDoll.DollController.FoodManager.SetAllPet(m_AllDollCharacters);
            m_ActiveDoll.DollController.FoodManager.ConstructDollCom(m_Inventory);


            m_ActiveDoll.DollController.BattleManager.AssignTurretCamera(m_ProperCamera); 
                
            

            m_ActiveDoll.FillStats(m_AllDollCharacters.GetDoll(m_ActiveDollID));
            m_ActiveDoll.DollController.GoToBed(m_AllDollSleeps.GetDoll(m_ActiveDoll.DollID));

            m_ActiveDoll.FillCombatStats(m_AllDollBattle.GetDoll(m_ActiveDollID));

            m_Camera.SetTarget(m_ActiveDoll.transform);
            m_ShipInputController.SetTargetDoll(m_ActiveDoll.DollController);
            m_GaitInputController.SetCurrentDoll(m_ActiveDollController);


            OnActiveDollChanged?.Invoke(index);
            //m_Dashboard.SetDoll(m_ActiveDoll);

            m_TrailDolls = new Doll[2];

            m_NotFirstDoll = true;
        }

        // 5viii26
        public void InitCurrentDoll()
        {
            InitDoll(m_ActiveDollIndexInParty);
        }

        // 5. Методы отряда как системы кукол


        // Изменение выносливости
        public void ChangeStamina(int deltaStam)
        {
            m_Stamina += deltaStam;
            m_Stamina = Mathf.Clamp(m_Stamina, 0, 37);
        }

        // Восстановление выносливости



        // Изменение вида камеры
        private bool a_ = false;
        public void LookAtWisp()
        {
            a_ = !a_;

            if (a_)
            {
                m_Camera.SetTarget(transform);
            }
            if (!a_)
            {
                m_Camera.SetTarget(m_ActiveDoll.transform);
            }
        }


        /// <summary>
        // Установить патрулирование (караван)
        /// Всё то же, что и раньше в SetDollsPatrol,
        /// только без m_ActiveDoll.DollController.SetDollAsActive(true); и без
        ///    m_Camera.SetTarget(m_ActiveDoll.transform);
        ///    m_ShipInputController.SetTargetDoll(m_ActiveDoll.DollController);
        ///    m_GaitInputController.SetCurrentDoll(m_ActiveDollController);
        ///    Куклы синхронизируются, а не патрулируют в обычном смысле
        /// </summary>
        public void SetTrailDolls()
        {
            if (m_ProcessionMode)
            {
                DestroyImmediate(m_TrailDolls[0].gameObject); 
                DestroyImmediate(m_TrailDolls[1].gameObject);

                m_ProcessionMode = false;
            }
            else
            {

                m_ProcessionMode = true;

                m_TrailDolls[0] = Instantiate(m_DollPrefabs[m_DollsCode[(m_ActiveDollIndexInParty + 1) % 3]]);
                m_TrailDolls[1] = Instantiate(m_DollPrefabs[m_DollsCode[(m_ActiveDollIndexInParty + 2) % 3]]);


                m_TrailDolls[0].DollController.AIDollFollower.SetForeDoll(m_ActiveDoll.PetAsSpaceShip);
                m_TrailDolls[1].DollController.AIDollFollower.SetForeDoll(m_TrailDolls[0].PetAsSpaceShip);
            }
        }
        

private void SetDollsPatrol()
        {
            //m_ActiveDoll.GetComponent<AIController>().ResetPatrolBehaviour();
            //foreach (var character in m_PartyControllerList)
            //{
            //    if (character != m_ActiveDollController && !character.Sleeping)
            //    {
            //        // character.GetComponent<AIController>().SetPatrolBehaviour(m_ActiveDoll.Wisp);

            //        character.GetComponent<AIController>().SetPatrolBehaviour(transform.GetComponent<AIPointPatrol>());
            //        character.NavelEffect(true);
            //    }
            //    else
            //    {
            //        character.NavelEffect(false);
            //    }
            //}
        }


        // Переместить часть или всех кукол
        public void PlaceSomeOrAllDolls(int[] pos)
        {
            if (m_ActiveDoll == null) return;

            m_ActiveDoll.DollController.PositionManager.Fill(pos);

            print($"Abrunho!! {pos[1]} {pos[3]}");
        }

        // Взять прошлую позицию кукол и поставить их в данную точку


        // Пауза
        public void PauseAllDolls()
        {
            print("Pau");
            m_ActiveDoll.DollController.Pause();
        }

        // Конец паузы
        public void UnPauseAllDolls()
        {
            m_ActiveDoll.DollController.UnPause();
        }


        public void DollCarryWisp(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
        }

        public void RestoreHPAll(int healAmount)
        {
            m_AllDollBattle.RestoreHPAll(healAmount);
            m_ActiveDoll.PetAsSpaceShip.RestoreHitPoints(healAmount);
        }

        public void SetDollInParty(string dolladdr)
        {
            int index = int.Parse(dolladdr[..1]);
            int dollID = int.Parse(dolladdr[1..]);
            m_DollsCode[index] = dollID;

            m_CurrentPartyDolls.DollConsist[index] = dollID;
        }

        // 6. События Unity

        private void Awake()
        {
            m_GaitMap = new int[3] { 2, 2, 2 };


        }
        private void Start()
        {
        }
        private void Update()
        {
            if (m_ActiveDoll != null)
            {
                m_Visual.SetActive(true);
            }
            else
            {
                m_Visual.SetActive(false);
            }
        }   
        
        // 16.05.26 20:47
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
            }
            else
            {
                m_NotFirstDoll = false;
                m_NotSessionStart = false;
                InitBase(m_CurrentScene.LocationIndex, m_TimePastStats.ReadTime());

                m_IsSwimming = false;
            }
        }

       
    }

}
