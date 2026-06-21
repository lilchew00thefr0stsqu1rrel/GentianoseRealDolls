using Common;
using GentianoseRealDolls;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public class VirtualGamePad : MonoBehaviour
    {
        [SerializeField] private Party m_Party;
        [SerializeField] private MoveInputController m_MoveInputController;
        [SerializeField] private GaitInputController m_GaitInputController;

        public GameObject MobileGait;
        public UIButton MobileJump;
        public UIButton MobileLookAtWisp;
        public UIButton MobileNormalAttack;

        public VirtualJoystick VirtualJoystick;
        public VirtualJoystick VirtualJoystickRotation;

        public void GaitUp()
        {
            m_GaitInputController.GaitUp();
        }

        public void GaitDown()
        {
            m_GaitInputController.GaitDown();
        }

        public void Jump()
        {
            m_MoveInputController.Leap();
        }

        public void LookAtWisp()
        {
            m_Party.LookAtWisp();
        }
    }
}

