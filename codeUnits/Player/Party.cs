using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using VContainer;

namespace GentianoseRealDolls
{
    public class Party : MonoBehaviour
    {


        // TODO: сделать поля открытыми для UI
        // звери больше не зависят от UI, требования принципа DI
        // Каждые 0.1 с обновляется HUD

        // Делегат для события изменения здоровья
        public delegate void ActiveDollChanged(int dollIndexInParty);
        public event ActiveDollChanged OnActiveDollChanged;


        ////// public IObjectResolver _objectResolver;

        [Header("Services")]
        [SerializeField] private int m_MapID;
        [SerializeField] private CurrentSceneData m_CurrentScene;
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

        [SerializeField] private AllDollCharacters m_AllDollCharacters;
        [SerializeField] private AllDollPositions m_AllDollPositions;
        [SerializeField] private AllDollSleeps m_AllSleeps;
        [SerializeField] private AllDollBattle m_AllDollBattle;

        [SerializeField] private List<int> m_Stats;
        [SerializeField] private int[] m_Position;
        [SerializeField] private List<int> m_BedData;
        [SerializeField] private List<int> m_Combat;
        [SerializeField] private bool[] m_DirtyDolls;

        [SerializeField] private PoopStore m_PoopStore;


        [SerializeField] private Inventory m_Inventory;
        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }

        [Header("Dolls at the moment")]


        [SerializeField] private Doll[] m_DollPrefabs;

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

        [SerializeField] private Transform m_ParentOfLeadDoll;
        [SerializeField] private Transform m_ParentOfTrailDolls;


        //[SerializeField] private List<Doll> m_PartyList;
        //[SerializeField] private List<DollController> m_PartyControllerList;

        //[SerializeField] private Doll[] m_PartyMembers;

        [SerializeField] private int[] m_GaitMap;
        public int[] GaitMap => m_GaitMap;
        public void SetGaitMap(int[] gm)
        {
            m_GaitMap = gm;
        }

        [SerializeField] private Doll m_ActiveDoll;
        public Doll ActiveDoll => m_ActiveDoll;

        [Range(0.0f, 37.0f)]
        [SerializeField] private int m_Stamina;
        public int Stamina => m_Stamina;

        private DollController m_ActiveDollController;
        public DollController ActiveDollController => m_ActiveDollController;


        [SerializeField] private DollAsset[] m_DollAssets;


        private int m_ActiveDollIndexInParty;
        public int ActiveDollIndexInParty => m_ActiveDollIndexInParty;
        public bool AreThereSleepingBeasts => m_ActiveDoll.DollController.SleepSystem.IsSleeping;

        private long m_TimeDifference;

        public static event Action OnChangeActiveDoll;




        // Изменение выносливости
        public void ChangeStamina(int deltaStam)
        {
            m_Stamina += deltaStam;
            m_Stamina = Mathf.Clamp(m_Stamina, 0, 37);
        }

        // Восстановление выносливости
        private IEnumerator RestoreStaminaCorout()
        {
            yield return new WaitForSeconds(1);
            if (!m_ActiveDoll.DollController.GaitManager.IsMoving || m_ActiveDoll.DollController.GaitManager.GaitState < 3)
            {
                ChangeStamina(1);
            }
            StartCoroutine(RestoreStaminaCorout());
        }


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

        private void Awake()
        {
            // m_PartyMembers = new Doll[3];
            m_BedData = new List<int>();
            m_Position = new int[5];
            m_Stats = new List<int>();
            m_Combat = new List<int>();
            m_GaitMap = new int[3] { 2, 2, 2 };

            m_DirtyDolls = new bool[3];
            //StartCoroutine(TimeSave());
            //m_TimeDifference = m_TimePastStats.ReadTime();

            print("~~~ " + m_SimplePosDB.GetDollPosition(0).Position);

        }
        private void Start()
        {
            ReadDolls();
        }

        [SerializeField] private GameObject m_Visual;
        [SerializeField] float dH = 0.7f;
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

        public void DollCarryWisp(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
        }


        #region Position

        [SerializeField] private Vector3 m_Waypoint;

        [SerializeField] private bool m_FromWaypoint;


        [SerializeField] private DollBase m_SimplePosDB;



        // Переместить часть или всех кукол
        public void PlaceSomeOrAllDolls(int loc, Vector3 wp)
        {
            var pos = new int[5]{m_CurrentDollID, loc,
                (int)Mathf.Ceil(wp.x),
                (int)Mathf.Ceil(wp.y),
                (int)Mathf.Ceil(wp.z)};
            m_ActiveDoll.DollController.PositionManager.Fill(pos);

            print($"Abrunho!! {pos[2]} {pos[4]}" );
        }

        // Взять прошлую позицию кукол и поставить их в данную точку




        #endregion

        #region Initialization

        [SerializeField] private int sessionHouseMap = 0;

        // Создать кукол
        // Переменные:
        // локация (домик, город)
        // Файлы характеристик кукол, положения и сна (спит/бодрствует)
        // Разность времени, получаемая из Файлов времени 
        // Положение зверька в отряде

        // Интерфейс пользователя зависит от зверей, а не наоборот


        public void InitDolls(int mapID, long time)
        {
            sessionHouseMap++;

            //ReadDolls();

            SetActiveDoll(0);

            StartCoroutine(SaveDollsTick());


            StatsReduceOverTime();

            StartCoroutine(RestoreStaminaCorout());

            print("Pets are ready");
        }


        // 16.05.26 20:47
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                m_DirtyDolls = new bool[3];
            }
        }

        private int m_CurrentDollID = 0;

        private void ReadDolls()
        {
            m_Stats = m_AllDollCharacters.GetDolls();
            m_Position = m_AllDollPositions.GetDoll();
            m_BedData = m_AllSleeps.GetDolls();

            m_Combat = m_AllDollBattle.GetDolls();


            m_Inventory.InitInventory();

            m_TimeDifference = m_TimePastStats.ReadTime();

            m_PoopStore.InitPoop();

            print($"Welcome to location #{m_CurrentScene.LocationIndex}");

            print("whooo");
        }

        private int[] GetDollStats(int dollID)
        {
            var s = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                s.Add(m_Stats[dollID * 8 + i]);
            }
            return s.ToArray();
        }
        private int[] GetDollSleep(int dollID)
        {
            var s = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                s.Add(m_BedData[dollID * 2 + i]);
            }
            return s.ToArray();
        }
        //public void FillDolls(long time)
        //{
        //    SetActiveDoll(0);

        //    SetDollsPatrol();
        //}

        private IEnumerator SaveDollsTick()
        {
            yield return new WaitForSeconds(1);
            FetchDolls();

            StartCoroutine(SaveDollsTick());
        }

        private void FetchDolls()
        {
            var dollStats = m_ActiveDoll.FetchStats();
            m_AllDollCharacters.WriteDoll(dollStats);

            var po = m_ActiveDoll.DollController.PositionManager.Fetch();
            m_Position = po;
            m_AllDollPositions.SetDoll(po);

            var slp = m_ActiveDoll.DollController.SleepSystem.Fetch();
            m_AllSleeps.WriteDoll(slp);

            var cbt = m_ActiveDoll.FetchCombatStats();
            m_AllDollBattle.WriteDoll(cbt);
        }


        #endregion

        // Изменение шкал кукол по времени
        async void StatsReduceOverTime()
        {
            await Task.Delay(60000);

            m_ActiveDoll.DollController.ReduceStatsOverTime();



            StatsReduceOverTime();

            print("ToDrain");
        }


        [SerializeField] private bool[] m_PartyDollSleeps = new bool[3];

        public bool[] PartyDollSleeps => m_PartyDollSleeps;

        #region Dolls


        // Переключиться на данного персонажа
        // Сделать персонажей активным/ неактивными
        public void SetActiveDoll(int index)
        {
            ReadDolls();
            long time = m_TimePastStats.ReadTime();
            m_ActiveDollIndexInParty = index;


            if (m_ActiveDoll) DestroyImmediate(m_ActiveDoll.gameObject);
            m_ActiveDoll = Instantiate(m_DollPrefabs[index], m_ParentOfLeadDoll);

            m_ActiveDoll.DollController.SetDollAsActive(true);

            m_ActiveDoll.DollController.PositionManager.Fill(m_Position.ToArray());


            m_ActiveDollController = m_ActiveDoll.DollController;



            m_CurrentDollID = m_ActiveDoll.DollID;


            m_ActiveDoll.DollController.SetLocationIndex(m_MapID);
            m_ActiveDoll.DollController.ConstructDollParty(this);
            m_ActiveDoll.DollController.ConstructInventory(m_Inventory);
            m_ActiveDoll.DollController.ConstructPoop(m_PoopStore);
            m_ActiveDoll.DollController.SetDollProperties();


            var sts = GetDollStats(m_ActiveDoll.DollID);
            print("What the hell " + sts[6].ToString());
            var slp = GetDollSleep(m_ActiveDoll.DollID);

            // Нужно ли уменьшать шкалы на время вне игры?
            if (m_DirtyDolls[m_ActiveDoll.DollID] == false)
            {
                // В начале
                m_ActiveDoll.DollController.TimeActionStats(time, sts, slp);
                m_DirtyDolls[m_ActiveDoll.DollID] = true;
            }
            else
            {
                // При смене карты (домик/город)
                m_ActiveDoll.DollController.TakeStats(m_ActiveDollIndexInParty, sts);
            }

            m_Camera.SetTarget(m_ActiveDoll.transform);
            m_ShipInputController.SetTargetDoll(m_ActiveDoll.DollController);
            m_GaitInputController.SetCurrentDoll(m_ActiveDollController);


            OnActiveDollChanged?.Invoke(index);
            //m_Dashboard.SetDoll(m_ActiveDoll);

            SetDollsPatrol();
        }

        /// <summary>
        /// Всё то же, только без m_ActiveDoll.DollController.SetDollAsActive(true); и без
        ///    m_Camera.SetTarget(m_ActiveDoll.transform);
        ///    m_ShipInputController.SetTargetDoll(m_ActiveDoll.DollController);
        ///    m_GaitInputController.SetCurrentDoll(m_ActiveDollController);
        ///    Куклы синхронизируются, а не патрулируют в обычном смысле
        /// </summary>
        private void SetTrailDolls()
        {

        }


        // Установить патрулирование (караван)
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

        // Лечение 
        public void RestoreHPAll(int m_HealAmount)
        {
            //foreach (var chrct in m_PartyMembers)
            //{
            //    chrct.RestoreHP(m_HealAmount);
            //}

            m_Combat[1] += m_HealAmount;
            m_Combat[3] += m_HealAmount;
            m_Combat[5] += m_HealAmount;
        }

        // Лечение по времени (регенерация) в виде баффа
        public void RegenHPAll(int m_HealAmount)
        {
            //foreach (var chrct in m_PartyMembers)
            //{
            //    chrct.PetAsSpaceShip.ApplyBuff();
            //}
        }

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

        public void SetSleepDoll(int partyIndex, bool v)
        {
            m_PartyDollSleeps[partyIndex] = v;
        }
        #endregion

    }

}
