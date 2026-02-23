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

        private void Awake()
        {
            // gaitState = m_ActiveDoll.gaitState;
            gaitState = 2;
            print(gaitSpeeds[gaitState - 1]);

            print($"{spaceShip != null}, {spaceShip.MaxLinearVelocity}, {gaitSpeeds[gaitState - 1]}");
            spaceShip.SetMaxLinearVelocity(gaitSpeeds[gaitState - 1]);

            OnGaitTextUpdate += GaitDisplay.UpdateText();
          
        }

        private void OnDestroy()
        {

            OnGaitTextUpdate -= GaitDisplay.UpdateText();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
                     OnGaitTextUpdate(m_DollIndexInParty, gaitState);
         //   UpdateText();
            m_Animator = GetComponent<Animator>();
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
            
            if (m_Doll.DollController.ActiveDollInPartyStatus)
            {
                var animatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
                // смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
                if (animatorStateInfo.IsTag("+++"))
                {
                    if (animatorStateInfo.normalizedTime >= sprintThresholdCount)
                    {
                        print(m_Doll.name);
                        Party.Instance.ChangeStamina(-1);
                        sprintThresholdCount++;
                    }
                }
                else
                {
                    sprintThresholdCount = 1;
                }
              
            }
                    
            if (gaitState == 3 && Party.Instance.Stamina == 0 && isMoving)
            {
                Stop(); 
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

        

        public void Walk()
        {
            if (m_Doll.DollController.Sleeping) return;
            m_Animator.SetInteger("Autom", gaitCodes[0]);
            isMoving = true;


            MaybeSpecialGait();
        }
        public void SecondGait()
        {
            //     print("Canter");
            if (m_Doll.DollController.Sleeping) return;
            m_Animator.SetInteger("Autom", gaitCodes[1]);
            isMoving = true;

            MaybeSpecialGait();
        }

        public void Gallop()
        {
            if (Party.Instance.Stamina == 0) return;    
            if (m_Doll.DollController.Sleeping) return;
            m_Animator.SetInteger("Autom", gaitCodes[2]);
            isMoving = true;

            MaybeSpecialGait();  
        }

        public void MaybeSpecialGait()
        {
            if (m_Doll.DollController.BattleManager.LesserSkillBuff && gaitCodes[3] != 0)
            {
                print("Otter trot, bushbaby saltation");
                m_Animator.SetInteger("Autom", 30);


                spaceShip.SetMaxLinearVelocity(gaitSpeeds[3]);
            }
            
        }

        public void Stop()
        {
            if (m_Doll.DollController.Sleeping) return;
            print("тпру"); 
            if (m_Doll.DollController.BattleManager.LesserSkillBuff && gaitCodes[3] != 0)
            {
                m_Animator.SetInteger("Autom", 15);
            }
            else
            {
                m_Animator.SetInteger("Autom", 0);
            }
            isMoving = false;

        }
        
        public void StopGait()
        {
            var animatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            // смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
            if ((animatorStateInfo.IsTag("++") ||
               animatorStateInfo.IsTag("+++") ||
               animatorStateInfo.IsTag("+")) && animatorStateInfo.normalizedTime >= 8.0f)
                m_Doll.Sounds[7].Play();

            Stop();

        }



         public void SetGaitText(Text text)
         {
            m_GaitText = text;
         }

        public void SetGaitState(int gs)
        {
            if (gs >= 1 && gs <= 3)
            {

                gaitState = gs;
                spaceShip.SetMaxLinearVelocity(gaitSpeeds[gaitState - 1]);
                //             UpdateText();
                OnGaitTextUpdate(m_DollIndexInParty, gaitState);

                switch (gs)
                {
                    case 1:
                        Walk();
                        break;
                    case 2:
                        SecondGait();
                        break;
                    case 3:
                        Gallop();
                        break;
                }
            }
            else
            {
                print("This gait transmission is invalid");
            }
        }

        public void UpGaitState(int dollIndex)
        {
            if (gaitState < 3)
            {

                gaitState++;
                spaceShip.SetMaxLinearVelocity(gaitSpeeds[gaitState - 1]);
                //             UpdateText();
                //OnGaitTextUpdate(m_DollIndexInParty, gaitState);

                OnGaitTextUpdate(dollIndex, gaitState);

                print("цок");
            }
        }
        public void DownGaitState(int dollIndex)
        {
            if (gaitState > 1)
            {

                gaitState--;
                spaceShip.SetMaxLinearVelocity(gaitSpeeds[gaitState - 1]);
                //   UpdateText();
                // OnGaitTextUpdate(m_DollIndexInParty, gaitState);

               OnGaitTextUpdate(dollIndex, gaitState);


                print("фифю");
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
        }
    }
}
