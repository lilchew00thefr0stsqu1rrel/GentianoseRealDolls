using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class GaitInputController : MonoBehaviour
    {

        [SerializeField] private Doll m_CurrentDoll;
        [SerializeField] private Animator m_Animator;


        [SerializeField] private SpaceShip spaceShip;


        [SerializeField] private int[] gaitCodes = new int[4];
        [SerializeField] private float[] gaitSpeeds = new float[4];
        private bool addTime;
        private float timer;
        private bool isMoving;

        private bool m_AtTransition;

   

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            UpdateGait();
            //UpdateMovement();
        }

        public void SetCurrentDoll(Doll doll)
        {
            m_CurrentDoll = doll;
            m_Animator = doll.EntityAnimator;
            spaceShip = doll.PetAsSpaceShip;
        }
        void UpdateGait()
        {
            var doll = m_CurrentDoll;

            if (doll)
            {
                var gaitManager = doll.DollController.GaitManager;


               // print(gaitManager.PartyDollID);

                if (!doll.DollController.Sleeping)
                {

                    if (Input.GetKeyDown(KeyCode.LeftControl) && !m_AtTransition)
                    {
                        gaitManager.DownGaitState(Party.Instance.ActiveDollIndexInParty);
                    }

                    if (Input.GetKeyDown(KeyCode.LeftShift) && !m_AtTransition)
                    {
                        gaitManager.UpGaitState(Party.Instance.ActiveDollIndexInParty);

                    }


                    if (Input.GetKeyDown(KeyCode.W))
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
                            m_CurrentDoll.Sounds[1].Play();
                        }

                        if (gaitManager.GaitState == 3)
                        {
                            m_CurrentDoll.Sounds[2].Play();
                        }

                    }



                    if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift))
                        && Input.GetKey(KeyCode.W))
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
        public Action<int> SetPartyDollNumber()
        {
            return (slot) =>
            {
                m_CurrentDoll.DollController.GaitManager.PartyDollID = slot;
            };
        }

        public static event Action<int, int> OnGaitTextUpdate;

        IEnumerator WaitGait()
        {
            m_AtTransition = true;
            yield return new WaitForSeconds(0.3f);
            m_AtTransition = false;
        }

      
    }
}

