using SpaceShooter;
using Unity.VisualScripting;
using UnityEngine;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(Doll))]
    public class DollController : MonoBehaviour
    {
        private Level m_Level;
        private AllDollCharacters allDolls;
        private AllDollSleeps allSleeps;
       

        [Header("Doll Base Attributes")]
        [SerializeField] private Doll m_Doll;
        public Doll Doll => m_Doll;
        [SerializeField] private Animator m_Animator;
        public Animator Animator => m_Animator;
        [SerializeField] private BeastPositionManager positionManager;

        [Header("Doll Component")]


        [SerializeField] private DollGaitManager gaitGear;
        public DollGaitManager GaitManager => gaitGear;

        [SerializeField] private DollBattleManager battler;
        public DollBattleManager BattleManager => battler;

        [SerializeField] private DollPoopManager pooper;
        public DollPoopManager PoopManager => pooper;

        [SerializeField] private DollBath bathSystem;

        [SerializeField] private DollSleep sleepSystem;

        [SerializeField] private DollClimbing climbing;
        public DollClimbing Climbing => climbing;
        
        [Header("Doll's particular parts, e.g. weapons and scent glands")]
        [SerializeField] private DollPart[] parts;

        private Dashboard m_Dashboard;
        private Party m_Party;
        public bool Sleeping => sleepSystem.Sleeping;

        private int m_DollIndexInParty;

        public int DollIndexInParty => m_DollIndexInParty;
        public void SetDollIndexInParty(int index)
        {
            m_DollIndexInParty = Mathf.Clamp(index, 0, 2);


            InitAllDollComponents();
        }

        public void SetIdle()
        {
            m_Animator.SetInteger("Autom", 0);
        }



        private void Awake()
        {
           

            dollID = m_Doll.DollID; 
            InitAllDollComponents();

            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void InitAllDollComponents()
        {
            gaitGear.SetProperties(m_Doll, m_Animator, m_DollIndexInParty);
            pooper.SetProperties(m_Doll, m_Animator, m_DollIndexInParty);
            battler.SetProperties(m_Doll, m_Animator, m_DollIndexInParty);
            sleepSystem.SetProperties(m_Doll, m_Animator, m_DollIndexInParty);
            bathSystem.SetProperties(m_Doll, m_Animator, m_DollIndexInParty);

        }

        private void Start()
        {
            foreach (DollPart dp in parts)
            {
                if (dp is DollClimbing)
                    climbing = (DollClimbing)dp;
            }
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                sleepSystem.WakeDoll(m_DollIndexInParty);
            }
        }
        private DollScaleValues m_ScaleValues;
        private int dollID;
        private Rigidbody m_Rigidbody;

        [SerializeField] private bool m_IsActiveDollInParty;
        public bool ActiveDollInPartyStatus => m_IsActiveDollInParty;

        public bool FullSleep => m_Doll.FullSleep;

        public void SetActiveDoll(bool active)
        {
            m_IsActiveDollInParty = active;
        }
        public void TimeActionStats(long timeDifference, int partyIndex)
        {
            m_ScaleValues = allDolls.GetDollData(dollID);

            bool isSleeping = allSleeps.GetSleepingByID(m_Doll.DollID);

            if (isSleeping)
            {
                sleepSystem.GoToBed(partyIndex);
            }
            else
            {
                sleepSystem.WakeDoll(partyIndex);
            }

            if (sleepSystem.Sleeping)
            {
                sleepSystem.GoToBed(partyIndex);


                m_Doll.SetSleep(m_ScaleValues.Sleep + timeDifference);

                positionManager.WarpDoll(m_Level.Beds[dollID]);
                
            }

            else
            {

                print("Snegura");
                print("Snegura"+ m_ScaleValues.Sleep + timeDifference);
                m_Doll.SetSleep(m_ScaleValues.Sleep - timeDifference);
                print("Snegure");
            }

            float poo = Mathf.Clamp(m_ScaleValues.LooPoo - timeDifference * Doll.StepLooStat, 0, Doll.MaxLooStat);

            float analSpray = Mathf.Clamp(m_ScaleValues.AnalSprayAmount +
                timeDifference * (Doll.StepAnalGlandSecretions * m_Doll.AnalGlandVolume), 0, m_Doll.AnalGlandVolume);

            float pee = Mathf.Clamp(m_ScaleValues.LooPee - timeDifference * Doll.StepLooStat, 0, Doll.MaxLooStat);

            float bath = Mathf.Clamp(m_ScaleValues.Bath - timeDifference * Doll.StepBath, 0, Doll.MaxBrushTeeth);
            float brushTeeth = Mathf.Clamp(m_ScaleValues.BrushTeeth - timeDifference * Doll.StepBrushTeeth, 0, Doll.MaxBrushTeeth);

            m_Doll.SetToiletStats(poo, 
                analSpray, 
                pee, 
                bath,
                brushTeeth);

            float food = Mathf.Clamp(m_ScaleValues.FoodHunger - timeDifference, 0, Doll.MaxStat);

            m_Doll.SetFoodHunger(food);

        }

        public void TakeStats(int partyIndex)
        {
            m_ScaleValues = allDolls.GetDollData(dollID);

            bool isSleeping = allSleeps.GetSleepingByID(m_Doll.DollID);

            if (isSleeping)
            {
                sleepSystem.GoToBed(partyIndex);
            }
            else
            {
                sleepSystem.WakeDoll(partyIndex);
            }

            if (sleepSystem.Sleeping)
            {
                sleepSystem.GoToBed(partyIndex);


                m_Doll.SetSleep(m_ScaleValues.Sleep);

                positionManager.WarpDoll(m_Level.Beds[dollID]);

            }

            else
            {

                print("Snegura");
                print("Snegura" + m_ScaleValues.Sleep);
                m_Doll.SetSleep(m_ScaleValues.Sleep);
                print("Snegure");
            }
            float poo = Mathf.Clamp(m_ScaleValues.LooPoo, 0, Doll.MaxLooStat);

            float analSpray = Mathf.Clamp(m_ScaleValues.AnalSprayAmount, 0, m_Doll.AnalGlandVolume);

            float pee = Mathf.Clamp(m_ScaleValues.LooPee, 0, Doll.MaxLooStat);

            float bath = Mathf.Clamp(m_ScaleValues.Bath, 0, Doll.MaxBrushTeeth);
            float brushTeeth = Mathf.Clamp(m_ScaleValues.BrushTeeth, 0, Doll.MaxBrushTeeth);

            m_Doll.SetToiletStats(poo,
                analSpray,
                pee,
                bath,
                brushTeeth);

            float food = Mathf.Clamp(m_ScaleValues.FoodHunger, 0, Doll.MaxStat);

            m_Doll.SetFoodHunger(food);
        }

        public void GoToBed()
        {
            sleepSystem.GoToBed(m_DollIndexInParty);

            m_Doll.GetComponent<AIController>().SleepPatrolBehaviour();


        }
        public void WakeDoll()
        {
            if (!Sleeping) return;

            sleepSystem.WakeDoll(m_DollIndexInParty);

            if (!m_Doll.ActiveDollInPartyStatus)
                GetComponent<AIController>().WakePatrolBehaviour();


        }

        public void Wash()
        {
            bathSystem.Wash();
        }

        public void BrushTeeth()
        {
            bathSystem.BrushTeeth();
        }

        public void SetDollPosFromWaypoint(int loc, Vector3 waypoint, int index)
        {
            positionManager.SetDollPosFromWaypoint(loc, waypoint, index);
        }
        public void TakeAndSetDollPos(int loc, int index)
        {
            positionManager.TakeAndSetDollPos(loc, index);
        }

        public void ResetDollLocation()
        {
            positionManager.ResetLocation();
        }
        
        
        public void ConstructDolls(AllDollCharacters dolls, AllDollSleeps sleeps, Level level, Dashboard dashboard, Party party)
        {
            allDolls = dolls;
            allSleeps = sleeps;
            m_Level = level;
            m_Dashboard = dashboard;
            m_Party = party;

            positionManager.ConstructDolls(dolls, sleeps, level);
            sleepSystem.ConstructSleep(sleeps);

            gaitGear.ConstructDollCom(m_Dashboard, m_Party);
            battler.ConstructDollCom(m_Dashboard, m_Party);

            pooper.ConstructDollCom(m_Dashboard, m_Party);

            bathSystem.ConstructDollCom(m_Dashboard, m_Party);
            sleepSystem.ConstructDollCom(m_Dashboard, m_Party);

            // battler.SetHealSideParty(m_Party);

            foreach (var dp in parts)
            {
                if (dp != null)
                    dp.Construct(this, m_Party, m_Dashboard, m_Animator);
            }
        }

        public void StatsReduce()
        {

            print("Drain");

            sleepSystem.ApplySleep();

            m_Doll.ReduceNonSleepStats();
        }



        public void Pause()
        {
            m_Animator.enabled = false;
            m_Rigidbody.isKinematic = true;
        }

        public void UnPause()
        {
            m_Animator.enabled = true;
            m_Rigidbody.isKinematic = false;
        }


        bool m_NavelEffect;
        [SerializeField] private GameObject m_NavelEffectPrefab;
        private GameObject m_NavelEffectShield;
        
        public Transform Nose => climbing.Nose;  
        public void NavelEffect(bool enab)
        {
            m_NavelEffect = enab;
            m_Doll.PetAsSpaceShip.NavelEffect(enab);
            if (enab)
            {
                if (m_NavelEffectShield != null) return;

                m_NavelEffectShield = Instantiate(m_NavelEffectPrefab, transform);
                m_NavelEffectShield.transform.SetParent(transform, false);
            }
            else
            {
                Destroy(m_NavelEffectShield);
            }
        }

        public void SetClimb(int climbMode)
        {
            if (climbMode == 0)
            {
                climbing.EndClimbing();
            }
            if (climbMode == 1)
            {
                climbing.StartClimbing();
            }
            if (climbMode == 2)
            {
                climbing.StartDescend();
            }
        }

        public void StartClimbing()
        {
            climbing.StartClimbing();
        }

        public void StopClimbing()
        {
            climbing.EndClimbing() ;
        }

        public void StartDescend()
        {
            climbing.StartDescend();
        }
    }
}

