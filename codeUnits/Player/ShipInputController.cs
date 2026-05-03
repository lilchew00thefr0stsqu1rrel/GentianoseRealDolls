using GentianoseRealDolls;
using NTC.MonoCache;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter
{
    public class ShipInputController : MonoCache
    {
        [SerializeField] private ControlModeData controlModeData;
       
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

       

        public void Construct(VirtualGamePad virtualGamePad)
        {
            m_VirtualGamePad = virtualGamePad;
        }

        //private bool m_Landed;

        [SerializeField] private SpaceShip m_TargetShip;

        [SerializeField] private VirtualGamePad m_VirtualGamePad;
        [SerializeField] private CombatDashboard m_CombatDashboard;

        private void Start()
        {


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
                // ControlKeyboard();
                ControlNew();

            if (controlModeData.Control == ControlModeData.ControlMode.Mobile)
                ControlMobile();

            if (controlModeData.Control == ControlModeData.ControlMode.KeyboardAndMobile)
            {
                ControlKeyboardAndMobile();
            }
        }
        private void ControlNew()
        {

            float thrust = 0;
            float torque = 0;

            thrust = moveInput.y;
            torque = -moveInput.x;


            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
        }
        
        private void ControlMobile()
        {
            Vector3 dir = m_VirtualGamePad.VirtualJoystick.Value;

            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;
        }
        [SerializeField] private Animator m_Animator;
        //lemur
        public static bool mouseTorque = true;

        public void Leap()
        {
            m_TargetShip.Leap();
        }

        Vector2 moveInput;
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

            // В качестве наглядного эффекта
            // просто поворачиваем объект на 90 градусов
            Leap();
        }

        private void ControlKeyboard()
        {

            float thrust = 0;
            float torque = 0;

            float jump = 0;

            if (Input.GetKey(KeyCode.W))
                thrust = 1.0f;

            if (Input.GetKey(KeyCode.S))
                thrust = -1.0f;

            if (Input.GetKey(KeyCode.A))
                torque = 1.0f;

            if (Input.GetKey(KeyCode.D))
                torque = -1.0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {

                // m_TargetShip.Fire(TurretMode.Primary);
                jump = 1.0f;
                m_TargetShip.Leap();
            }

            if (Input.GetKey(KeyCode.X))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

          

            if (mouseTorque)
            {
                if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse X") < 0.6f)
                {
                    torque = -1.0f;
                }
                if (Input.GetAxis("Mouse X") < 0 && Input.GetAxis("Mouse X") > -0.6f)
                {
                    torque = 1.0f;
                }

             
            }
         
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                mouseTorque = !mouseTorque;
            }
            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
            m_TargetShip.JumpControl = jump;    
        }

        private void ControlKeyboardAndMobile()
        {            
                ControlKeyboard();          
                ControlMobile();
        }

     
    }
}

