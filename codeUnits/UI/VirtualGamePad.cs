using Common;
using GentianoseRealDolls;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public class VirtualGamePad : MonoBehaviour
    {
        [SerializeField] private Party m_Party;

        // [Inject]
        //public void Construct(Party obj)
        //{
        //    m_Party = obj;
        //}
        // public PointerClickHold MobileFirePrimary;
        // public PointerClickHold MobileFireSecondary;

        public GameObject MobileGait;
        public UIButton MobileJump;
        public UIButton MobileLookAtWisp;
        public UIButton MobileNormalAttack;

        public VirtualJoystick VirtualJoystick;

        public void GaitUp()
        {
            m_Party.GaitUp();
        }

        public void GaitDown()
        {
            m_Party.GaitDown();
        }

        public void Jump()
        {
            m_Party.Jump();
        }

        public void LookAtWisp()
        {
            m_Party.LookAtWisp();
        }
    }
}

