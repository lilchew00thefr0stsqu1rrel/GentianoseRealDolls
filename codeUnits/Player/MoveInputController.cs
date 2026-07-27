using NTC.MonoCache;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GentianoseRealDolls
{
    public class MoveInputController : MonoCache
    {
        [Header("***")]
        [SerializeField] private ControlModeData controlModeData;

        [SerializeField] private VirtualGamePad m_VirtualGamePad;
        [SerializeField] private CombatDashboard m_CombatDashboard;
        [SerializeField] private Party m_Party;

        [Header("***")]

        [SerializeField] private SpaceShip m_TargetShip;
        [SerializeField] private DollController m_TargetDoll;
        [SerializeField] private Animator m_Animator;

        [SerializeField] private GaitInputController m_GaitInputController;
        [SerializeField] private CameraAroundDoll m_CameraAroundDoll;

        //lemur
        public static bool mouseTorque = true;

        Vector2 moveInput;
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;


        public void SetTargetDoll(DollController doll)
        {
            m_TargetDoll = doll;
            m_TargetShip = doll.PetAsSpaceShip;
        }

        public void Construct(VirtualGamePad virtualGamePad)
        {
            m_VirtualGamePad = virtualGamePad;
        }

        //private bool m_Landed;

        private void Start()
        {
            m_TargetShip = m_Party.ActiveDoll.PetAsSpaceShip;

            if (controlModeData.Control == ControlModeData.ControlMode.Keyboard)
            {
                m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(false);

                m_VirtualGamePad.MobileGait.SetActive(false);

                m_VirtualGamePad.MobileJump.gameObject.SetActive(false);
                m_VirtualGamePad.MobileLookAtWisp.gameObject.SetActive(false);
                m_VirtualGamePad.MobileNormalAttack.gameObject.SetActive(false);

                m_CombatDashboard.NormalAttackButtonScreen.gameObject.SetActive(true);
            }            
                          
            else
            {
                m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(true);

                m_VirtualGamePad.MobileGait.SetActive(true);

                m_VirtualGamePad.MobileNormalAttack.gameObject.SetActive(true);
                m_VirtualGamePad.MobileJump.gameObject.SetActive(true);
                m_VirtualGamePad.MobileLookAtWisp.gameObject.SetActive(true);

                m_CombatDashboard.NormalAttackButtonScreen.gameObject.SetActive(false);
            }         
        }
        protected override void Run()
        {
            if (m_TargetShip == null) return;


            if (controlModeData.Control == ControlModeData.ControlMode.Keyboard)
                ControlNew();

            if (controlModeData.Control == ControlModeData.ControlMode.Mobile)
                ControlMobile();

            if (controlModeData.Control == ControlModeData.ControlMode.KeyboardAndMobile)
            {
                ControlKeyboardAndMobile();
            }

            m_Party.DollCarryWisp(m_TargetShip.transform.position + Vector3.up, m_TargetShip.transform.rotation);


        }


        private void ControlNew()
        {
            float thrust = 0;
            float torque = 0;

            if (moveInput != Vector2.zero)
            {
                thrust = moveInput.y;
                torque = -moveInput.x;
                
                m_CameraAroundDoll.Normalize();
            }

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;

            
            m_TargetDoll?.UpdateMoveInput(moveInput);
        }

        private float m_MobileXQuotient = 0.3f;

        private void ControlMobile()
        {
            m_TargetShip.ThrustControl = 0;
            m_TargetShip.TorqueControl = 0;

            Vector3 dir = m_VirtualGamePad.VirtualJoystick.Value;

            if (dir != Vector3.zero)
            {
                m_TargetShip.ThrustControl = dir.y;
                m_TargetShip.TorqueControl = -dir.x * m_MobileXQuotient;

                m_GaitInputController.StartGait();

                m_CameraAroundDoll.Normalize();
            }
            else
            {
                m_GaitInputController.StopGait();
            }
        }

        public void Leap()
        {
            m_TargetShip.Leap();
        }

        // Этот метод вызывается Player Input,
        // когда срабатывает действие Move
        public void OnMove(InputAction.CallbackContext context)
        {
            // Считываем значение Vector2 из Input System
            // Оно описывает направление движения
            moveInput = context.ReadValue<Vector2>();

            
        }  
      

        // Этот метод вызывается при нажатии кнопки Jump
        public void OnJump(InputAction.CallbackContext context)
        {
            // Проверяем, что действие именно выполнено,
            // а не отменено или в процессе
            if (!context.performed) return;

            Leap();
        }

       

        private void ControlKeyboardAndMobile()
        {
            ControlNew();
            ControlMobile();

            m_GaitInputController.Move(moveInput);
        }

    }
}

