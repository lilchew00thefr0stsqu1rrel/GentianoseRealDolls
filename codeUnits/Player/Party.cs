using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TowerDefense;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GentianoseRealDolls
{
    public class Party : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;


        // TODO: ������� ���� ��������� ��� UI
        // ����� ������ �� ������� �� UI, ���������� �������� DI
        // ������ 0.1 � ����������� HUD

        // ������� ��� ������� ��������� ��������
        public delegate void ActiveDollChanged(int dollIndexInParty);
        public event ActiveDollChanged OnActiveDollChanged;


        ////// public IObjectResolver _objectResolver;

        [Header("Services")]
        [SerializeField] private int m_MapID;
        [SerializeField] private CurrentSceneData m_CurrentScene;
        [SerializeField] private TimePastStats m_TimePastStats;
        [SerializeField] private TeleportBeasts m_TeleportBeasts;

        [SerializeField] private FollowCamera m_Camera;
        public FollowCamera Camera => m_Camera;
        [SerializeField] private Camera m_ProperCamera;
        [SerializeField] private GaitInputController m_GaitInputController;
        [SerializeField] private MoveInputController m_ShipInputController;


        [Header("UI")]

        [SerializeField] private CombatDashboard m_CombatDashboard;
        [SerializeField] private HabitatInterface m_HabitatInterface;


        [Header("Dependencies")]
         
        [SerializeField] private AllDollCharacters m_AllDollCharacters;
        [SerializeField] private DollScaleValues[] m_DollData;
        [SerializeField] private AllDollPositions m_AllDollPositions;
        [SerializeField] private AllDollSleeps m_AllSleeps;
        [SerializeField] private List<bool> m_SleepData;
        [SerializeField] private PoopStore m_PoopStore;


        [SerializeField] private Inventory m_Inventory;
        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }

        [Header("Dolls at the moment")]

        [SerializeField] private Doll[] m_DollPrefabs;

        [SerializeField] private List<Doll> m_PartyList;
        [SerializeField] private List<DollController> m_PartyControllerList;

        [SerializeField] private Doll[] m_PartyMembers;

        [SerializeField] private bool[] m_Sleeps;
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
        public bool AreThereSleepingBeasts => m_PartyList[0].DollController.Sleeping ||
            m_PartyList[1].DollController.Sleeping ||
            m_PartyList[2].DollController.Sleeping;

        private long m_CurrentTime;
        private long m_PreviousTime;

        private long m_TimeDifference;

        public static event Action OnChangeActiveDoll;
       



        // ��������� ������������
        public void ChangeStamina(int deltaStam)
        {
            m_Stamina += deltaStam;
            m_Stamina = Mathf.Clamp(m_Stamina, 0, 37);
        }

        // �������������� ������������
        private IEnumerator RestoreStaminaCorout()
        {
            yield return new WaitForSeconds(1);
            if (!m_ActiveDoll.DollController.GaitManager.IsMoving || m_ActiveDoll.DollController.GaitManager.GaitState < 3)
            {
                ChangeStamina(1);
            }
            StartCoroutine(RestoreStaminaCorout());
        }

        // ������ (��� -> ���� -> �����)
        public void GaitUp()
        {
            m_GaitInputController?.GaitUp();
        }

        // ������ (����� -> ���� -> ���) 
        public void GaitDown()
        {
            m_GaitInputController?.GaitDown();
        }

        // ������
        public void Jump()
        {
            m_ShipInputController?.Leap();
        }


        // ��������� ���� ������
        private bool a_ = false;
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
            m_PartyMembers = new Doll[3];
            m_Sleeps = new bool[3] {false, false, false};
            m_GaitMap = new int[3] {2, 2, 2} ;

            //StartCoroutine(TimeSave());
            //m_TimeDifference = m_TimePastStats.ReadTime();

            print("~~~ " + m_SimplePosDB.GetDollPosition(0).Position);
        }
        
        // ��������� ���� ����� �� �������
        IEnumerator TimeSave()
        {
            yield return new WaitUntil(() => DateTime.Now.Second == 59);

            yield return new WaitForSeconds(1);
            //            
            ReducePartyStats();

            StartCoroutine(TimeSave());
            print("ToDrain");
        }

        private void OnDestroy()
        {
        }
        [SerializeField] private GameObject m_Visual;
        [SerializeField] float dH = 0.7f;
        private void Update()
        {
            if (m_PartyList[m_ActiveDollIndexInParty] != null)
            {
                m_Visual.SetActive(true);
                transform.position = m_PartyList[m_ActiveDollIndexInParty].transform.position + Vector3.up * dH;
                transform.forward = m_PartyList[m_ActiveDollIndexInParty].transform.forward;
            }
            else
            {
                transform.position = Vector3.zero;  
                m_Visual.SetActive(false);
            }

            //if (Input.GetMouseButtonDown(1))
            //{
            //    a_ = !a_;

            //    if (a_)
            //    {
            //        m_Camera.SetTarget(transform);
            //        m_Camera.SetMinOffsetWisp();
            //    }
            //    if (!a_)
            //    {

            //        m_Camera.SetTarget(m_ActiveDoll.transform);
            //        m_Camera.SetMinOffsetDoll();
            //    }
            //}
          
        }


        #region Position

        [SerializeField] private Vector3 m_Waypoint;

        [SerializeField] private bool m_FromWaypoint;


        [SerializeField] private DollBase m_SimplePosDB;

        

        // ����������� ����� ��� ���� �����
        public void PlaceSomeOrAllDolls(int loc, Vector3 wp)
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
        } 
        
        // ����� ������� ������� ����� � ��������� �� � ������ �����
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

        // ������� �����
        // ����������:
        // ������� (�����, �����)
        // ����� ������������� �����, ��������� � ��� (����/����������)
        // �������� �������, ���������� �� ������ ������� 
        // ��������� ������� � ������

        // ��������� ������������ ������� �� ������, � �� ��������

        public void InitPoop(PoopStore poopStore)
        {
            m_PoopStore = poopStore;
            m_PoopStore.InitPoop();
        }

        public void InitInventory(Inventory inventory)
        {
            m_Inventory = inventory;
            m_Inventory.InitInventory();
        }

        public void InitDollStats(AllDollCharacters adc)
        {
            m_TimeDifference = m_TimePastStats.ReadTime();
            m_AllDollCharacters = adc;
            m_DollData = adc.ReadStats();
        }

        public void InitDollPos(int mapID, AllDollPositions adp)
        {
            m_MapID = mapID;
            adp.InitPositions();
        }

        public void InitDollSleep(List<bool> sleepData, AllDollSleeps ads)
        {
            m_SleepData = sleepData;
            ads.InitSleeps();
            m_AllSleeps = ads;
        }

        public void InitDolls(int mapID, long time)
        {
            InitControllerDoll(time);

            TakeDollsToLastPoint(mapID);

            print("Pets are ready");
        }

        public void InitDolls(int mapID, long time, Vector3 waypoint)
        {
            InitControllerDoll(time);


            PlaceSomeOrAllDolls(mapID, waypoint);
        }

        // 16.05.26 20:47
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                sessionHouseMap = 0;
            }
        }

        private void InitControllerDoll(long time)
        {

            sessionHouseMap++;


            m_PartyList.Clear();
            m_PartyControllerList.Clear();
            print($"Welcome to location #{m_CurrentScene.LocationIndex}");


            for (int i = 0; i < m_DollPrefabs.Length; i++)
            {
                var doll = Instantiate(m_DollPrefabs[i]);
                print($"Hello! I'm {doll.DollSpecies} My name is {doll.CharacterName}");

                doll.DollController.SetLocationIndex(m_MapID);

                //doll.DollController.ConstructDoll(dollData, adc, adp, sleepData, ads, this);
                doll.DollController.ConstructDollParty(this);

                doll.DollController.ConstructDollStats(m_DollData, m_AllDollCharacters, m_AllDollPositions, m_Inventory);
                doll.DollController.ConstructInventory(m_Inventory);
                doll.DollController.ConstructPos(m_AllDollPositions, m_AllSleeps);
                doll.DollController.ConstructSleep(m_SleepData, m_AllSleeps);
                doll.DollController.ConstructPoop(m_PoopStore);
                doll.DollController.SetDollProperties();

                // ����� �� ��������� ����� �� ����� ��� ����?
                // � ������
                if (sessionHouseMap <= 1)
                    doll.DollController.TimeActionStats(time, i, m_TeleportBeasts, m_DollData, m_AllSleeps);
                // ��� ����� ����� (�����/�����)
                if (sessionHouseMap > 1)
                    doll.DollController.TakeStats(i, m_AllDollCharacters, m_AllSleeps);

                m_PartyList.Add(doll);
                m_PartyControllerList.Add(doll.DollController);


                doll.DollController.SetDollIndexInParty(i);
            }

            m_PartyMembers = m_PartyList.ToArray();

            StartCoroutine(TimeSave());

            SetActiveDoll(0);


            m_Camera.SetTarget(m_ActiveDoll.transform);

            m_ShipInputController.SetTargetDoll(m_ActiveDollController);
            m_GaitInputController.SetCurrentDoll(m_ActiveDollController);

           

            SetDollsPatrol();

            print("whooo");

            WriteDollSleepState(m_PartyList);

            StartCoroutine(RestoreStaminaCorout());

        }


        #endregion

        [SerializeField] private bool[] m_PartyDollSleeps = new bool[3];
        public bool[] PartyDollSleeps => m_PartyDollSleeps;

        #region Dolls

        // ������ ��������� ��� (����/����������)
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


        // ������������� �� ������� ���������
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

        // ������� ���������� ��������/ �����������
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

        // ���������� �������������� (�������)
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

        // ��������� �������� �������������

        public void ReducePartyStats()
        {
            if (m_PartyMembers == null)
            {
                print("NO DOLL");
                return;
            }

            foreach (var chrct in m_PartyMembers)
            {
                chrct.DollController.StatsReduce();
            }
        }

        // ������� 
        public void RestoreHPAll(int m_HealAmount)
        {
            foreach (var chrct in m_PartyMembers)
            {
                chrct.RestoreHP(m_HealAmount);
            }
        }

        // ������� �� ������� (�����������) � ���� �����
        public void RegenHPAll(int m_HealAmount)
        {
            foreach (var chrct in m_PartyMembers)
            {
                chrct.PetAsSpaceShip.ApplyBuff();
            }
        }

        // �����
        public void PauseAllDolls()
        {
            print("Pau");
            for (int i = 0; i < m_PartyControllerList.Count; i++)
            {
                m_PartyControllerList[i].Pause();
            }
        }

        // ����� �����
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
