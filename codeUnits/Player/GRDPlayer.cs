using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class GRDPlayer : Player
    {
        private const long ticksInSecond = 10000000;

        public const long StatsPeriod = 60;

        private string fileName = "time.dat";


        [SerializeField] private Doll m_ActiveDoll;
        public Doll ActiveDoll { get { return m_ActiveDoll; } }




        [SerializeField] private Doll[] party = new Doll[3];
        [SerializeField] List<Doll> m_PartyList = new List<Doll>();
        public Doll[] PlayerParty => party;

        [SerializeField] private FollowCamera m_Camera;
        private ShipInputController movementController;

        public event Action<Doll> OnEnterScene;
        [SerializeField] private HabitatInterface m_HabitatMenuPrefab;
        [SerializeField] private DollBattleManager m_BattleManager;

      //  [SerializeField] private Party m_Party_;

        [SerializeField] private Level m_StartLevel;

        private long m_Time;
        private long m_PrevTime;

       


        [SerializeField] private GaitDisplay m_GaitDisplay;




        private event Action<int> OnDollNumberInParty;

        private new void Awake()
        {
            base.Awake();

            dollParty = new List<int>();
            

            Saver<long>.TryLoad(fileName, ref m_Time);

            Doll.PreviousTime = m_Time;
            //
            //   StartCoroutine(TimeSave());
            //
            Level.OnEnterNewLevel += GoEnterLevel();

        }
       
        IEnumerator TimeSave()
        {
            m_Time = DateTime.Now.Ticks / 600000000;

            Saver<long>.Save(fileName, m_Time);



            yield return new WaitUntil(() => DateTime.Now.Second  == 59);

            yield return new WaitForSeconds(1);
            //            
            if (Party.Instance != null)
            {
                Party.Instance.ReducePartyStats();
            }
            StartCoroutine(TimeSave());
            print("ToDrain");


        }

      

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //m_StartLevel.AddDoll(ActiveDoll);


        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Party.Instance.SetActiveDoll(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Party.Instance.SetActiveDoll(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Party.Instance.SetActiveDoll(2);
            }
        }

        //event/./
        [SerializeField] private List<int> dollParty;

        public static Level Level;

        public void SetLevel(Level level)
        {
            Level = level;
        }

        public void SetSpawnPoint(Transform sp)
        {
            m_SpawnPoint = sp;
        }

        private event Action OnPlayerAppear;

        public Action<Level> SetSpawnPointFromLevel(Level level)
        {
            return (level) =>
            {
                Level = level;
                m_SpawnPoint = level.SpawnPoint;

                EnterLevel();
            };
        }


        [SerializeField] HabitatInterface hab;

        [SerializeField] private Vector3[] m_AppearPositions;
        //--> LevelSpawnPoint.dat

        public Action<int> GoEnterLevel()
        {
            return (loc) =>
            {
                EnterLevel();
            };
        }

        [SerializeField] private int m_LocationID;

        private int m_WasInThisLoc;
  //      [SerializeField] private UnityEngine.UI.Text m_GaitText;
        //public void InitTeam()
        //{
        //    party = (Instance as GRDPlayer).party;
        //    m_DollPrefabs = (Instance as GRDPlayer).m_DollPrefabs;
        //    print("Начало ");

        //    if (m_LocationID == 0)
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            Destroy(party[i].gameObject);
        //            party[i] = Instantiate(m_DollPrefabs[i]);
        //            party[i].TakeAndSetDollPos(m_LocationID);
        //        }

        //        m_ActiveDoll = party[0];

        //    }



       
        //}

        public Action ExitLocation()
        {
            return () =>
            {
                //foreach (var dl in m_PartyList)
                //{

                //    Destroy(dl.gameObject);
                //}
                //m_PartyList = null;
            };
        }

        public void EnterLevel()
        {
            print("ent");

            m_LocationID = Level.LevelID;
          

            movementController = Instance.GetComponent<ShipInputController>();


            print(Level.name);

            print(SceneHelper.GameMode);

            party = Party.Instance.GetDolls();
            
         
         




            m_ActiveDoll = Party.Instance.ActiveDoll;


            movementController.SetTargetShip(m_ActiveDoll.PetAsSpaceShip);

            m_GaitDisplay.SetDolls(party);
          //  m_GaitDisplay.UpdateText();

           // party[0]. GaitManager.PartyDollID = 0;
            //party[1].GaitManager.PartyDollID = 1;

            m_Camera = (Instance as GRDPlayer).m_Camera;

            m_Camera.transform.position = Level.SpawnPoint.position - m_Camera.transform.forward;
            m_Camera.SetTarget(m_ActiveDoll.transform);

            SetDollsPatrol(0);

            if (SceneHelper.GameMode == Mode.Habitat)
            {
                Dashboard.Instance.SetDollHabitat(m_ActiveDoll);
                m_ActiveDoll.DollController.PoopManager.SetPoopStore();
            }
            if (SceneHelper.GameMode == Mode.OpenWorld)
            {
               // m_ActiveDoll.SetAnusAimCamera(m_Camera.GetComponent<Camera>());
                Dashboard.Instance.SetDollOpenWorld(m_ActiveDoll, m_Camera.ProperCamera);
                Dashboard.Instance.SetCamera(m_Camera.ProperCamera);

                
            }

           // m_ActiveDoll.SetAnimator();

            
            


        }


        private void ChangeActiveDoll(int index)
        {

            m_ActiveDoll = party[index];
            movementController.SetTargetShip(m_ActiveDoll.PetAsSpaceShip);
            m_Camera.SetTarget(m_ActiveDoll.transform);
            m_GaitDisplay.SetActiveDoll(m_ActiveDoll);
            SetDollsPatrol(index);


        }

        
        private void SetDollsPatrol(int index)
        {
            party[index].GetComponent<AIController>().ResetPatrolBehaviour();
            for (int i = 0; i < party.Length; i++)
            {
                if (i != index)
                {
                    party[i].GetComponent<AIController>().SetPatrolBehaviour(m_ActiveDoll.Wisp);
                }
            }
            
        }

        public void AfterEnterLevel()
        {
           
        }

    }
}

