using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GentianoseRealDolls
{
    /// <summary>
    /// Компонент, позволяющий изменять аллюры зверька
    /// По умолчанию - шаг, рысь, галоп
    /// Но так как у нас большинство играбельных зверей 
    /// представлены куньими, грызунами и приматами,
    /// вместо рыси чаще всего используются четырёхтактная иноходь
    /// ("лисья рысь")
    /// или трёхтактный галоп - кентер
    /// </summary>
    public class DollGaitManager : DollComponent
    {



        [Range(1f, 3f)]
        [SerializeField] private int gaitState;

        private int[] m_DollGaits;

        //[SerializeField] private Doll m_Doll;
        //[SerializeField] private Animator m_Animator;


        [SerializeField] private SpaceShip spaceShip;


        [SerializeField] private int[] gaitCodes = new int[4];
        [SerializeField] private float[] gaitSpeeds = new float[4];





        [SerializeField] private Text m_GaitText;


        const string oneCross = "+";
        const string twoCrosses = "++";
        const string threeCrosses = "+++";


        public int GaitState => gaitState;

        int m_GaitAnimation;
        public int GaitAnimation => m_GaitAnimation;

        private void Awake()
        {
            // gaitState = m_ActiveDoll.gaitState;
            gaitState = 2;
            print(gaitSpeeds[gaitState - 1]);

            print($"{spaceShip != null}, {spaceShip.MaxLinearVelocity}, {gaitSpeeds[gaitState - 1]}");
            spaceShip.SetMaxLinearVelocity(gaitSpeeds[gaitState - 1]);

            // OnGaitTextUpdate += GaitDisplay.Instance.UpdateText();

            
          
        }

        private void OnDestroy()
        {

          //  OnGaitTextUpdate -= GaitDisplay.Instance.UpdateText();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            //OnGaitTextUpdate(m_DollIndexInParty, gaitState);

            // GaitDisplay.Instance.TextChange(m_DollIndexInParty, gaitState); 
            //   UpdateText();
            //m_Animator = GetComponent<Animator>();
        }

        //public void GaitTextUpdateSubscribe()
        //{
        //    OnGaitTextUpdate += GaitDisplay.UpdateText();
        //    OnGaitTextUpdate(m_DollIndexInParty, gaitState);
        //}

        // Update is called once per frame

        public int PartyDollID;
        private float sprintCount = 0;
        private int sprintThresholdCount = 1;
        private void Update()
        {
            //   UpdateGait();
            //UpdateMovement();

            if (!m_Doll) return;
            
            if (m_Doll.DollController.ActiveDollInPartyStatus)
            {
                // смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
                if (m_AnimatorGuard.IsGallop())
                {
                    if (m_AnimatorGuard.NormalizedTime() >= sprintThresholdCount)
                    {
                        print(m_Doll.name);
                        m_Party.ChangeStamina(-1);
                        sprintThresholdCount++;
                    }
                }
                else
                {
                    sprintThresholdCount = 1;
                }
              
            }
                    
            if (gaitState == 3 && m_Party.Stamina == 0 && isMoving)
            {
                StopGait();
            }

         

        }

        private bool isMoving;
        public bool IsMoving => isMoving;
      

        private bool m_AtTransition;

       

        public Action<int> SetPartyDollNumber()
        {
            return (slot) =>
            {
                PartyDollID = slot;
            };
        }

        public static event Action<int, int> OnGaitTextUpdate;

        IEnumerator WaitGait()
        {
            m_AtTransition = true;
            yield return new WaitForSeconds(0.3f);
            m_AtTransition = false;
        }

        #region Gait Motion


        public void Walk()
        {
            if (m_Doll.DollController.SleepSystem.IsSleeping) return;
            m_AnimatorGuard.SetAnimation(gaitCodes[0]);
            isMoving = true;

            m_GaitAnimation = gaitCodes[0];

            spaceShip.SetMaxLinearVelocity(gaitSpeeds[0]);

            MaybeSpecialGait();
        }
        public void SecondGait()
        {
            if (m_Doll.DollController.Sleeping) return;
            m_AnimatorGuard.SetAnimation(2);
            isMoving = true;


            m_GaitAnimation = gaitCodes[1];

            spaceShip.SetMaxLinearVelocity(gaitSpeeds[1]);

            MaybeSpecialGait();
        }

        public void Gallop()
        {
            if (m_Party.Stamina == 0) return;    
            if (m_Doll.DollController.Sleeping) return;
            m_AnimatorGuard.SetAnimation(3);
            isMoving = true;

            m_GaitAnimation = gaitCodes[2];

            spaceShip.SetMaxLinearVelocity(gaitSpeeds[2]);

            MaybeSpecialGait();  
        }

        public void MaybeSpecialGait()
        {
            if (m_Doll.DollController.BattleManager.LesserSkillBuff && gaitCodes[3] != 0)
            {
                print("Otter trot, bushbaby saltation");
                m_AnimatorGuard.SetAnimation(30);

                m_GaitAnimation = gaitCodes[3];

                spaceShip.SetMaxLinearVelocity(gaitSpeeds[3]);
            }

        }

        public void StartGait()
        {
            print(gaitCodes[1]);
            if (gaitState == 1)
                Walk();
            if (gaitState == 2)
                SecondGait();
            if (gaitState == 3)
                Gallop();


            if (m_Doll.DollController.BattleManager.LesserSkillBuff && gaitCodes[3] != 0)
            {
                MaybeSpecialGait();
            }


            if (gaitState == 2)
            {
                m_Doll.Sounds[1].Play();
            }

            if (gaitState == 3)
            {
                m_Doll.Sounds[2].Play();
            }
            print("Doll sets its gait");

            isMoving = true;
        }

        public void StopGait()
        {
            if (m_AnimatorGuard)
            {

                // смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
                if (m_AnimatorGuard.IsMotion() && m_AnimatorGuard.NormalizedTime() >= 8.0f)

                    m_Doll.Sounds[7].Play();

                if (m_Doll.DollController.Sleeping) return;

                if (!m_AnimatorGuard.IsMotion()) return;

                print("тпру");
                if (gaitCodes[3] != 0 && m_Doll.DollController.BattleManager.LesserSkillBuff)
                {
                    m_AnimatorGuard.SetAnimation(9);
                }
                else
                {
                    m_AnimatorGuard.SetAnimation(0);
                }
                isMoving = false;
            }
        }

        #endregion

        #region Gait Change

        public void SetGaitState(int gs)
        {
            if (gs >= 1 && gs <= 3)
            {
                gaitState = gs;
                spaceShip.SetMaxLinearVelocity(gaitSpeeds[gaitState - 1]);
            }
            else
            {
                print("This gait transmission is invalid");
            }
        }

        #endregion
    }
}
