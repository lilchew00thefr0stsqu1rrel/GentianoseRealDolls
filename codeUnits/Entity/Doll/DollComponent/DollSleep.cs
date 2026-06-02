using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class DollSleep : DollComponent
    {

        [SerializeField] private AllDollSleeps allSleeps;
        public void ConstructSleep(AllDollSleeps sleeps)
        {
            allSleeps = sleeps;
        }

        public override void SetProperties(Doll doll, AnimatorGuard animatorGuard, int posInParty)
        {
            base.SetProperties(doll, animatorGuard, posInParty);


           // m_IsSleeping = allSleeps.GetDollInBed(m_Doll.DollID);
        }
        

        [SerializeField] private bool m_IsSleeping;
        public bool Sleeping => m_IsSleeping;
        public void GoToBed(int partyIndex)
        {
            print("Sleep");
            m_AnimatorGuard.SetAnimation(10);
            m_IsSleeping = true;

            MoveInputController.mouseTorque = false;


            print(m_DollIndexInParty + "  / " + m_Doll.name);
            allSleeps.WriteDollSleep(m_Doll.DollID, true);

            m_Party.SetSleepDoll(partyIndex, true);
        }
        public void WakeDoll(int partyIndex)
        {
            print("dndlr");
            m_AnimatorGuard.SetAnimation(0);
            m_IsSleeping = false;


            MoveInputController.mouseTorque = true;



            allSleeps.WriteDollSleep(m_Doll.DollID, false);

            m_Party.SetSleepDoll(partyIndex, false);

        }

        public bool FullSleep => m_Doll.Sleep >= Doll.MaxStat;

        public void ApplySleep()
        {
            m_Doll.ChangeSleep(m_IsSleeping);
        }


       
    }

}
