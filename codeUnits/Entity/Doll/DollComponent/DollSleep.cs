using SpaceShooter;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class DollSleep : DollComponent
    {
        private void Awake()
        {
        }

        private void Start()
        {
            m_IsSleeping = AllDollSleeps.Instance.GetDollInBed(m_Doll.DollID);

            
        }

        [SerializeField] private bool m_IsSleeping;
        public bool Sleeping => m_IsSleeping;
        public void GoToBed(int partyIndex)
        {
            print("Sleep");
            m_Animator.SetInteger("Autom", 10);
            m_Animator.SetBool("TailCoiled", true);
            m_IsSleeping = true;

            ShipInputController.mouseTorque = false;


            

            print(m_DollIndexInParty + "  / " + m_Doll.name);
            AllDollSleeps.Instance.WriteDollSleep(m_Doll.DollID, true);

            Dashboard.Instance.SetSleepDoll(partyIndex, true);

            // GetComponent<AIController>().SleepPatrolBehaviour();
        }
        public void WakeDoll(int partyIndex)
        {
            print("dndlr");
            m_Animator.SetInteger("Autom", 0);
            m_Animator.SetBool("TailCoiled", false);
            m_IsSleeping = false;

            ShipInputController.mouseTorque = true;


            AllDollSleeps.Instance.WriteDollSleep(m_Doll.DollID, false);

            Dashboard.Instance.SetSleepDoll(partyIndex, false);
            //if (!m_Doll.ActiveDollInPartyStatus)
            //    GetComponent<AIController>().WakePatrolBehaviour();
        }

        public bool FullSleep => m_Doll.Sleep >= Doll.MaxStat;

        public void ApplySleep()
        {
            m_Doll.ChangeSleep(m_IsSleeping);
        }


       
    }

}
