using System;
using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class DollSleep : DollComponent
    {
        [SerializeField] private int[] m_Sleeping;
        public int[] Sleeping => m_Sleeping;

        [SerializeField] private bool m_IsSleeping;
        public bool IsSleeping => m_IsSleeping;

        public static event Action<int[]> OnSleepOrWake;

        public override void SetProperties(Doll doll, AnimatorGuard animatorGuard, int posInParty)
        {
            base.SetProperties(doll, animatorGuard, posInParty);
        }
        private void Awake()
        {
            m_Sleeping = new int[2];
        }



        public void SetSleep(int[] sleepState)
        {
            m_Sleeping = sleepState;
            m_IsSleeping = m_Sleeping[1] == 1;

            if (sleepState[1] == 1)
            {
                m_AnimatorGuard.SetAnimation(10);
                MoveInputController.mouseTorque = false;
            }
            else if (sleepState[1] == 0)
            {
                m_AnimatorGuard.SetAnimation(0);
                MoveInputController.mouseTorque = true;
            }

            OnSleepOrWake(m_Sleeping);
        }

        public bool FullSleep => m_Doll.Sleep >= Doll.MaxStat;

        public void ApplySleep()
        {
            m_Doll.ChangeSleep(m_Sleeping[1] == 1);
        }

        public int[] Fetch() 
        { 
            return m_Sleeping;

        }
    }

}
