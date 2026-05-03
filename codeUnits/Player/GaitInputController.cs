using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;

using NTC.MonoCache;
using Unity.VisualScripting;
using System.Threading.Tasks;

namespace GentianoseRealDolls
{
    public class GaitInputController : MonoCache, IDependency<Party>
    {
        private Party m_Party;
        public void Construct(Party obj)
        {
            m_Party = obj;
            m_CurrentDoll = m_Party.ActiveDoll;
        }
        // Делегат для события изменения аллюра
        public delegate void GaitChanged(int indexInParty, int gaitState);
        public event GaitChanged OnGaitChanged;

        [SerializeField] private Doll m_CurrentDoll;
        [SerializeField] private DollController m_CurrentDollController;
        [SerializeField] private Animator m_Animator;


        [SerializeField] private SpaceShip spaceShip;

        
        [SerializeField] private int[] m_GaitMap = new int[3] { 2, 2, 2 };

        [SerializeField] private int[] gaitCodes = new int[4];
        [SerializeField] private float[] gaitSpeeds = new float[4];
        private bool addTime;
        private float timer;
        private bool isMoving;

        private bool m_AtTransition;

        [SerializeField] private VirtualGamePad m_VirtualGamePad;


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
            UpdateGait();


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

        public void SetCurrentDoll(DollController doll)
        {
           // m_CurrentDoll = doll.Doll;
            m_CurrentDollController = doll;
            m_Animator = doll.Animator;
            spaceShip = doll.Doll.PetAsSpaceShip;


            m_ActiveDollInPartyIndex = doll.DollIndexInParty;
        }

        [SerializeField] private int m_ActiveDollInPartyIndex;

        private void SetDollGait(DollGaitManager gm, int dollIndexInParty, int gaitState)
        {
            if (!m_AtTransition)
            {
                m_GaitMap[m_ActiveDollInPartyIndex] = Mathf.Clamp(gaitState, 1, 3);

                //gaitManager.DownGaitState(Party.Instance.ActiveDollIndexInParty);
                gm.SetGaitState(m_GaitMap[m_ActiveDollInPartyIndex]);

                OnGaitChanged?.Invoke(dollIndexInParty, gaitState);


                if (gaitState == 1) m_Party.Camera.BirdEye(); else m_Party.Camera.ReBirdEye();
            }
          
        }

        public async void GaitUp()
        {;
            if (!m_AtTransition)
            {
                SetDollGait(m_CurrentDollController.GaitManager, m_ActiveDollInPartyIndex, m_GaitMap[m_ActiveDollInPartyIndex] + 1);
                m_AtTransition = true;

                await Task.Delay(300);
                m_AtTransition = false;
            }
        }
        public async void GaitDown()
        {
            if (!m_AtTransition)
            {
                SetDollGait(m_CurrentDollController.GaitManager, m_ActiveDollInPartyIndex, m_GaitMap[m_ActiveDollInPartyIndex] - 1); m_AtTransition = true;

                await Task.Delay(300);
                m_AtTransition = false;
            }
        }

        void UpdateGait()
        {
            //var doll = m_CurrentDoll;

            if (m_CurrentDollController)
            {
                var gaitManager = m_CurrentDollController.GaitManager;


               // print(gaitManager.PartyDollID);

                if (!m_CurrentDollController.Sleeping)
                {

                    //if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        //SetDollGait(gaitManager, m_ActiveDollInPartyIndex, m_GaitMap[m_ActiveDollInPartyIndex] - 1);

                        //print("la");

                    }

                    //if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                    //    SetDollGait(gaitManager, m_ActiveDollInPartyIndex, m_GaitMap[m_ActiveDollInPartyIndex] + 1);


                    //    print("tui");
                        //m_GaitMap[m_ActiveDollInPartyIndex] = Mathf.Clamp(m_GaitMap[m_ActiveDollInPartyIndex] + 1, 1, 3);
                        ////gaitManager.UpGaitState(Party.Instance.ActiveDollIndexInParty);
                        //gaitManager.SetGaitState(m_GaitMap[m_ActiveDollInPartyIndex]);

                    }


                    if ((Input.GetKeyDown(KeyCode.W)  || m_VirtualGamePad.VirtualJoystick.Value.y > 0)
                        || (gaitManager.GaitState == 1 && 
                       (Input.GetKeyDown(KeyCode.S) || m_VirtualGamePad.VirtualJoystick.Value.y < 0)))
                    {
                        print(gaitCodes[1]);
                        if (gaitManager.GaitState == 1)
                            gaitManager.Walk();
                        if (gaitManager.GaitState == 2)
                            gaitManager.SecondGait();
                        if (gaitManager.GaitState == 3)
                            gaitManager.Gallop();

                        if (gaitManager.GaitState == 2)
                        {
                            
                            m_CurrentDoll?.Sounds[1].Play();
                        }

                        if (gaitManager.GaitState == 3)
                        {
                            m_CurrentDoll?.Sounds[2].Play();
                        }

                    }



                    if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(2))
                        && (Input.GetKey(KeyCode.W) || m_VirtualGamePad.VirtualJoystick.Value.y > 0))
                    {
                        gaitManager.StartGait();
                    }

                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        gaitManager.StopGait(); 
                    }
                }
            }
        }

        [SerializeField] private BoxCollider m_ColliderClimb;
        [SerializeField] private BoxCollider m_ColliderOutClimb;

       
        public Action<int> SetPartyDollNumber()
        {
            return (slot) =>
            {
                m_CurrentDoll.DollController.GaitManager.PartyDollID = slot;
            };
        }


    }
}


