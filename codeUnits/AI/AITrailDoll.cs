using NTC.MonoCache;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class AITrailDoll : MonoCache
    {
        [SerializeField] private bool m_IsTrailDoll;
        [SerializeField] private SpaceShip m_SpaceShip;
        [SerializeField] private SpaceShip m_ForeDoll;

        [SerializeField] private float m_Thrust;
        [SerializeField] private float m_Torque;

        public void SetForeDoll(SpaceShip fD)
        {
            m_IsTrailDoll = true;
            m_ForeDoll = fD;

            transform.position = m_ForeDoll.transform.position - m_ForeDoll.transform.forward;
        }

        protected override void FixedRun()
        {
            if (m_IsTrailDoll && m_SpaceShip  != null && m_ForeDoll != null)
            {
                m_SpaceShip.ThrustControl = m_ForeDoll.ThrustControl;

                m_SpaceShip.TorqueControl = m_ForeDoll.TorqueControl;

                transform.forward = m_ForeDoll.transform.forward;
            }

        }
    }
}
