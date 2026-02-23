using SpaceShooter;
using UnityEngine;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(Doll))]
    public class DollController : MonoBehaviour
    {
        [Header("Doll Base Attributes")]
        [SerializeField] private Doll m_Doll;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private BeastPositionManager positionManager;

        [Header("Doll Component")]
        [SerializeField] private DollGaitManager gaitGear;
        public DollGaitManager GaitManager => gaitGear;
        [SerializeField] private DollPoopManager pooper;
        public DollPoopManager PoopManager => pooper;
        [SerializeField] private DollBattleManager battler;
        public DollBattleManager BattleManager => battler;
        [SerializeField] private DollSleep sleepSystem;
        [SerializeField] private DollBath bathSystem;

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
            m_ScaleValues = AllDollCharacters.Instance.GetDollData(dollID);

            bool isSleeping = AllDollSleeps.GetSleepingByID(m_Doll.DollID);
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
               // m_Doll.WarpDoll(Level.Instance.Beds[m_Doll.DollID]);
                sleepSystem.GoToBed(partyIndex);


                m_Doll.SetSleep(m_ScaleValues.Sleep + timeDifference);



                
                positionManager.WarpDoll(Level.Instance.Beds[dollID]);
                
            }

            else
            {

                m_Doll.SetSleep(m_ScaleValues.Sleep - timeDifference);
            }

            float poo = Mathf.Clamp(m_ScaleValues.LooPoo - timeDifference * Doll.StepLooStat, 0, Doll.MaxLooStat);

            float analSpray = Mathf.Clamp(m_Doll.AnalSprayAmount +
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

        public void GoToBed()
        {
            sleepSystem.GoToBed(m_DollIndexInParty);

            m_Doll.GetComponent<AIController>().SleepPatrolBehaviour();


        }
        public void WakeDoll()
        {
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
    }
}

