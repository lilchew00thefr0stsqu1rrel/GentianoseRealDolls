using NTC.MonoCache;
using SpaceShooter;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace GentianoseRealDolls
{
    public class GaitInputController : MonoCache
    {
        [Header("***")]
        [SerializeField] private Party m_Party;

        [SerializeField] private VirtualGamePad m_VirtualGamePad;



        ////[Inject]
        //public void Construct(Party obj)
        //{
        //    m_Party = obj;
        //    m_CurrentDoll = m_Party.ActiveDoll;
        //}
        // Делегат для события изменения аллюра
        public delegate void GaitChanged(int indexInParty, int gaitState);
        public event GaitChanged OnGaitChanged;

        [Header("***")]
        [SerializeField] private Doll m_CurrentDoll;
        [SerializeField] private DollController m_CurrentDollController;
        [SerializeField] private DollGaitManager m_GaitManager;
        [SerializeField] private Animator m_Animator;


        [SerializeField] private SpaceShip spaceShip;

        
        [SerializeField] private int[] m_GaitMap = new int[3] { 2, 2, 2 };
        public int[] GaitMap => m_GaitMap;

        [SerializeField] private int[] gaitCodes = new int[4];

        private bool addTime;
        private float timer;
        private bool isMoving;

        private bool m_AtTransition;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }
        private async void ShootGait()
        {
            await Task.Delay(1000);

            
        }

        protected override void Run()
        {
            //UpdateGait();


            OnGaitChanged?.Invoke(0, m_GaitMap[0]);
            OnGaitChanged?.Invoke(1, m_GaitMap[1]);
            OnGaitChanged?.Invoke(2, m_GaitMap[2]);
        }

        // Update is called once per frame
        private void Update()
        {
           //UpdateGait();
            //UpdateMovement();
        }

        [SerializeField] private int m_ActiveDollSize;
        public void SetCurrentDoll(DollController doll)
        {
           // m_CurrentDoll = doll.Doll;
            m_CurrentDollController = doll;
            m_Animator = doll.Animator;
            spaceShip = doll.Doll.PetAsSpaceShip;
            m_GaitManager = doll.GaitManager;

            m_ActiveDollInPartyIndex = doll.DollIndexInParty;

            m_ActiveDollSize = doll.Doll.Asset.ModelSize;

            m_Party.Camera.ReBirdEye(m_ActiveDollSize);
        }

        [SerializeField] private int m_ActiveDollInPartyIndex;

        

        private void SetDollGait(DollGaitManager gm, int dollIndexInParty, int gaitState)
        {
            if (!m_AtTransition)
            {
                m_GaitMap[dollIndexInParty] = Mathf.Clamp(gaitState, 1, 3);
                m_Party.SetGaitMap(m_GaitMap);

                gm.SetGaitState(m_GaitMap[m_ActiveDollInPartyIndex]);

                ////OnGaitChanged?.Invoke(dollIndexInParty, gaitState);


                if (gaitState == 1)
                {
                    m_Party.Camera.BirdEye();
                }
                else
                {
                    m_Party.Camera.ReBirdEye(m_ActiveDollSize);
                }

            }
        }
        
        public async void GaitUp()
        {
            if (!m_AtTransition)
            {
                SetDollGait(m_GaitManager, m_ActiveDollInPartyIndex, m_Party.GaitMap[m_ActiveDollInPartyIndex] + 1);
                m_AtTransition = true;

                await Task.Delay(200);
                m_AtTransition = false;

                if (moveInput.y > 0)
                {
                    m_GaitManager.StartGait();
                }
            }
        }
        public async void GaitDown()
        {
            if (!m_AtTransition)
            {
                SetDollGait(m_GaitManager, m_ActiveDollInPartyIndex, m_Party.GaitMap[m_ActiveDollInPartyIndex] - 1); 
                m_AtTransition = true;

                await Task.Delay(200);
                m_AtTransition = false;

                if (moveInput.y > 0)
                {
                    m_GaitManager.StartGait();
                }
            }
        }
        public void OnGaitUp(InputAction.CallbackContext context)
        {

            // Проверяем, что действие именно выполнено,
            // а не отменено или в процессе
            if (!context.performed) return;

            GaitUp();
        }
        public void OnGaitDown(InputAction.CallbackContext context)
        {

            // Проверяем, что действие именно выполнено,
            // а не отменено или в процессе
            if (!context.performed) return;

            GaitDown();
        }
        [SerializeField] private float m_TestWS;

        public void StartGait()
        {
            m_GaitManager?.StartGait();
        }
        public void StopGait()
        {
            m_GaitManager?.StopGait();
        }

        public void OnStartGait(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            StartGait();
        }
        public void OnStopGait(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            StopGait();
        }

        [SerializeField] Vector2 moveInput;

        public void Move(Vector2 move)
        {
            moveInput = move;
        }

         


       
        public Action<int> SetPartyDollNumber()
        {
            return (slot) =>
            {
                m_CurrentDoll.DollController.GaitManager.PartyDollID = slot;
            };
        }


    }
}

