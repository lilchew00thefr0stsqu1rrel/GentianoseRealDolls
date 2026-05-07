using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerDefense;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GentianoseRealDolls
{
    public class Party : MonoBehaviour, IParty
    {



        private const string fileName1 = "timeCurr.dat";
        private const string fileName2 = "timePrev.dat";

        // Делегат для события изменения здоровья
        public delegate void ActiveDollChanged(int dollIndexInParty);
        public event ActiveDollChanged OnActiveDollChanged;


        ////// public IObjectResolver _objectResolver;

        [Header("Services")]

        [SerializeField] private CurrentSceneData m_CurrentScene;
        [SerializeField] private TeleportBeasts m_TeleportBeasts;

        [SerializeField] private FollowCamera m_Camera;
        public FollowCamera Camera => m_Camera;
        [SerializeField] private Camera m_ProperCamera;
        [SerializeField] private GaitInputController m_GaitInputController;
        [SerializeField] private MoveInputController m_ShipInputController;


        [Header("UI")]

        [SerializeField] private CombatDashboard m_CombatDashboard;
        [SerializeField] private HabitatInterface m_HabitatInterface;


        [Header("Controls")]
         
        [SerializeField] private AllDollCharacters m_AllDollCharacters;
        [SerializeField] private AllDollPositions m_AllDollPositions;
        [SerializeField] private AllDollSleeps m_AllSleeps;

        [Header("Dolls at the moment")]

        [SerializeField] private Doll[] m_DollPrefabs;

        [SerializeField] private List<Doll> m_PartyList;
        [SerializeField] private List<DollController> m_PartyControllerList;

        [SerializeField] private Doll[] m_PartyMembers;

        [SerializeField] private bool[] m_Sleeps;

        [SerializeField] private Doll m_ActiveDoll;
        public Doll ActiveDoll => m_ActiveDoll;

        [Range(0.0f, 37.0f)]
        [SerializeField] private int m_Stamina;
        public int Stamina => m_Stamina;

        private DollController m_ActiveDollController;

        // [SerializeField] private Dashboard m_Dashboard;

        //private IDollSettable m_Dashboard;


        [SerializeField] private DollAsset[] m_DollAssets;


        private int m_ActiveDollIndexInParty;
        public int ActiveDollIndexInParty => m_ActiveDollIndexInParty;
        public bool AreThereSleepingBeasts => m_PartyList[0].DollController.Sleeping ||
            m_PartyList[1].DollController.Sleeping ||
            m_PartyList[2].DollController.Sleeping;

        private long m_CurrentTime;
        private long m_PreviousTime;

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

        // Аллюры
        public void GaitUp()
        {
            m_GaitInputController?.GaitUp();
        }
        // Аллюры
        public void GaitDown()
        {
            m_GaitInputController?.GaitDown();
        }
        // Прыжок
        public void Jump()
        {
            m_ShipInputController?.Leap();
        }
        private bool a_ = false;


        // Изменение вида камеры
        public void LookAtWisp()
        {
            a_ = !a_;

            if (a_)
            {
                m_Camera.SetTarget(transform);
                m_Camera.SetMinOffsetWisp();
            }
            if (!a_)
            {

                m_Camera.SetTarget(m_ActiveDoll.transform);
                m_Camera.SetMinOffsetDoll();
            }
        }


        private void Awake()
        {
            ////// _objectResolver.InjectGameObject(gameObject);

            m_PartyMembers = new Doll[3];

            m_Sleeps = new bool[3];


            StartCoroutine(TimeSave());

            Saver<long>.TryLoad(fileName1, ref m_CurrentTime);
            Saver<long>.TryLoad(fileName2, ref m_PreviousTime);
            print("Prev  " + m_PreviousTime);


            m_TimeDifference = m_CurrentTime - m_PreviousTime;

        }


        // Изменение шкал кукол по времени
        IEnumerator TimeSave()
        {
            m_CurrentTime = DateTime.Now.Ticks / 600000000;

            Saver<long>.Save(fileName1, m_CurrentTime);



            yield return new WaitUntil(() => DateTime.Now.Second == 59);

            yield return new WaitForSeconds(1);
            //            
            ReducePartyStats();
            
            StartCoroutine(TimeSave());
            print("ToDrain");


        }

        private void OnDestroy()
        {
            m_PreviousTime = m_CurrentTime;
            Saver<long>.Save(fileName2, m_PreviousTime);
        }

        private void Start()
        {

        }
        [SerializeField] float dH = 0.7f;
        private void Update()
        {
            if (m_PartyList[m_ActiveDollIndexInParty] != null)
            {
                transform.position = m_PartyList[m_ActiveDollIndexInParty].transform.position + Vector3.up * dH
                    + m_PartyList[m_ActiveDollIndexInParty].transform.forward;
                transform.forward = m_PartyList[m_ActiveDollIndexInParty].transform.forward;
            }

            if (Input.GetMouseButtonDown(1))
            {
                a_ = !a_;

                if (a_)
                {
                    m_Camera.SetTarget(transform);
                    m_Camera.SetMinOffsetWisp();
                }
                if (!a_)
                {

                    m_Camera.SetTarget(m_ActiveDoll.transform);
                    m_Camera.SetMinOffsetDoll();
                }
            }
          
        }


        #region Position

        [SerializeField] private Vector3 m_Waypoint;

        [SerializeField] private bool m_FromWaypoint;

        // Переместить кукол
        public void PlaceDolls(int loc, Vector3 wp)
        {

            m_Waypoint = wp;

            m_CurrentScene.SetLocationIndex(loc);

            for (int i = 0; i < m_PartyList.Count; i++)
            {
                if (m_PartyList[i] != null)
                {
                    m_PartyList[i].DollController.SetDollPosFromWaypoint(loc, m_Waypoint, i);
                }
            }


        }

        // Переместить часть кукол
        public void PlaceSomeDolls(int loc, Vector3 wp)
        {


            m_Waypoint = wp;


            for (int i = 0; i < m_PartyList.Count; i++)
            {
                if (m_PartyList[i] != null)
                {
                    if (!m_PartyControllerList[i].Sleeping)
                    {
                        m_PartyList[i].DollController.SetDollPosFromWaypoint(loc, m_Waypoint, i);
                    }
                }
            }


        } // Переместить часть кукол
        public void TakeDollsToLastPoint(int loc)
        {

            for (int i = 0; i < m_PartyList.Count; i++)
            {
                if (m_PartyList[i] != null)
                {
                    if (!m_PartyControllerList[i].Sleeping)
                    {
                        m_PartyList[i].DollController.TakeAndSetDollPos(loc, i);
                    }
                }
            }


        }




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
        

        public void InitDolls(int mapID, 
            AllDollCharacters adc, AllDollPositions adp, AllDollSleeps ads,
            long time)
        {
            InitControllerDoll(mapID, adc, adp, ads, time);

            TakeDollsToLastPoint(mapID);
        }

        public void InitDolls(int mapID,
            AllDollCharacters adc, AllDollPositions adp, AllDollSleeps ads,
            long time, Vector3 waypoint)
        {
            InitControllerDoll(mapID,
            adc, adp, ads, time);

            PlaceDolls(mapID, waypoint);
        }

        private void InitControllerDoll(int mapID,
            AllDollCharacters adc, AllDollPositions adp, AllDollSleeps ads,
            long time)
        {

            sessionHouseMap++;


            m_PartyList.Clear();
            m_PartyControllerList.Clear();
            print($"Welcome to location #{m_CurrentScene.LocationIndex}");

            ads.InitSleeps();

            for (int i = 0; i < m_DollPrefabs.Length; i++)
            {
                var doll = Instantiate(m_DollPrefabs[i]);

                doll.DollController.ConstructDoll(adc, adp, ads, this);


                m_PartyList.Add(doll);
                m_PartyControllerList.Add(doll.DollController);


                if (sessionHouseMap <= 1)
                    doll.DollController.TimeActionStats(m_TimeDifference, i);
                if (sessionHouseMap > 1)
                    doll.DollController.TakeStats(i);

                doll.DollController.SetDollIndexInParty(i);
            }

            m_PartyMembers = m_PartyList.ToArray();

            SetActiveDoll(0);


            m_Camera.SetTarget(m_ActiveDoll.transform);

            m_ShipInputController.SetTargetDoll(m_ActiveDollController);
            m_GaitInputController.SetCurrentDoll(m_ActiveDollController);

            SetDollsPatrol();

            print("whooo");

            WriteDollSleepState(m_PartyList);

            StartCoroutine(RestoreStaminaCorout());
        }

        public void InitDolls(int mapID)
        {
            sessionHouseMap++;


            m_PartyList.Clear();
            m_PartyControllerList.Clear();
            print($"Welcome to location #{m_CurrentScene.LocationIndex}");

            int index = 0;

            m_AllSleeps.InitSleeps();

            foreach (var dl in m_DollPrefabs)
            {
                print("Position " + m_AllDollPositions.GetDollPos(dl.DollID).Positions[mapID]);
                var doll = Instantiate(dl);
                ////// _objectResolver.Inject(doll);

                print($"{doll} Hoary!!");



                doll.DollController.ConstructDoll(m_AllDollCharacters, m_AllDollPositions, m_AllSleeps, this);


                if (m_Waypoint != Vector3.zero)
                {
                    doll.DollController.SetDollPosFromWaypoint(mapID, m_Waypoint, index);

                }
                else
                {
                    doll.DollController.TakeAndSetDollPos(mapID, index);
                }

                //// print($"Bushbaby 7{doll}");
                m_PartyList.Add(doll);
                m_PartyControllerList.Add(doll.DollController);
                index++;

            }


            

                //// Кц аварийнай яачть

                // Изменить значения шкал согласно пройденному времени вне игры 
              
            
            m_PartyMembers = m_PartyList.ToArray();

            SetActiveDoll(0);


            m_Camera.SetTarget(m_ActiveDoll.transform);

            m_ShipInputController.SetTargetDoll(m_ActiveDollController);
            m_GaitInputController.SetCurrentDoll(m_ActiveDollController);
            

            // print(m_Dashboard != null);
            print(m_ActiveDoll);
            print(m_Camera.ProperCamera != null);
            ////m_Dashboard.SetDoll(m_ActiveDoll);

            SetDollsPatrol();





            print("whooo");

            WriteDollSleepState(m_PartyList);

            StartCoroutine(RestoreStaminaCorout());
        }

        #endregion

        [SerializeField] private bool[] m_PartyDollSleeps = new bool[3];
        public bool[] PartyDollSleeps => m_PartyDollSleeps;

        #region Dolls

        // Запись состояния сна (спит/бодрствует)
        private Action WriteDollSleepState(List<Doll> dollParty)
        {
            return () =>
            {
                int count = 0;
                foreach (Doll doll in dollParty)
                {
                    m_AllSleeps.WriteDollSleep(doll.DollID, doll.DollController.Sleeping);

                    // m_Dashboard.SetSleepDoll(count, doll.DollController.Sleeping);

                    m_PartyDollSleeps[count] = doll.DollController.Sleeping;

                    count++;
                }
            };
        }


        // Переключиться на данного персонажа
        public void SetActiveDoll(int index)
        {
            m_ActiveDollIndexInParty = index;
            m_ActiveDoll = m_PartyMembers[index];
            m_ActiveDollController = m_ActiveDoll.DollController;

            SetDollsActive(index);

            m_Camera.SetTarget(m_ActiveDoll.transform);
            m_ShipInputController.SetTargetDoll(m_ActiveDoll.DollController);
            m_GaitInputController.SetCurrentDoll(m_ActiveDollController);


            OnActiveDollChanged?.Invoke(index);
            //m_Dashboard.SetDoll(m_ActiveDoll);

            SetDollsPatrol();
        }

        // Сделать персонажей активным/ неактивными
        private void SetDollsActive(int inPartyID)
        {
            foreach (var character in m_PartyControllerList)
            {
                if (character != m_ActiveDollController && !character.Sleeping)
                {
                    character.SetDollAsActive(false);
                }
                else
                {
                    character.SetDollAsActive(true);
                }
            }
        }

        // Установить патрулирование (караван)
        private void SetDollsPatrol()
        {
            m_ActiveDoll.GetComponent<AIController>().ResetPatrolBehaviour();
            foreach (var character in m_PartyControllerList)
            {
                if (character != m_ActiveDollController && !character.Sleeping)
                {
                    // character.GetComponent<AIController>().SetPatrolBehaviour(m_ActiveDoll.Wisp);

                    character.GetComponent<AIController>().SetPatrolBehaviour(transform.GetComponent<AIPointPatrol>());
                    character.NavelEffect(true);


                }
                else
                {

                    character.NavelEffect(false);
                }


                
            }
        }

        // Уменьшить значения характеристик

        public void ReducePartyStats()
        {
            foreach (var chrct in m_PartyMembers)
                chrct.DollController.StatsReduce();

        }

        // Лечение 
        public void RestoreHPAll(int m_HealAmount)
        {
            foreach (var chrct in m_PartyMembers)
            {
                chrct.RestoreHP(m_HealAmount);
            }
        }

        // Лечение по времени (регенерация) в виде баффа
        public void RegenHPAll(int m_HealAmount)
        {
            foreach (var chrct in m_PartyMembers)
            {
                chrct.PetAsSpaceShip.ApplyBuff();
            }
        }

        // Пауза
        public void PauseAllDolls()
        {
            print("Pau");
            for (int i = 0; i < m_PartyControllerList.Count; i++)
            {
                m_PartyControllerList[i].Pause();
            }
        }

        // Конец паузы
        public void UnPauseAllDolls()
        {
            for (int i = 0; i < m_PartyControllerList.Count; i++)
            {
                if (m_PartyControllerList[i] != null)   
                    m_PartyControllerList[i].UnPause();
            }
        }

        public void SetSleepDoll(int partyIndex, bool v)
        {
            m_PartyDollSleeps[partyIndex] = v;
        }
        #endregion



    }

}

