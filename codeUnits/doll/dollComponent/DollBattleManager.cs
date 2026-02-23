using Common;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;

namespace GentianoseRealDolls
{
    /// <summary>
    /// Боевые способности зверька.
    /// В качестве ультимативной способности используется распыление
    /// секрета анальных желез
    /// Ресурсом при этом служит масло в анальных железах
    /// Зверь пускает струю, как скунс
    /// В данной вселенной так делают не только куньи или скунсовые,
    /// но и приматы.
    /// </summary>
    public class DollBattleManager : DollComponent
    {

        private GRDTimer chargedTimer;
        [SerializeField] private float m_ChargedAttackTime = 0.592f;
        [SerializeField] private float m_ChargedAttackAnimationTime = 1.5f;
        [SerializeField] private float m_AttackTime = 0.2f;
        [SerializeField] private float m_SprayTime = 0.851f;
        [SerializeField] private float m_AttackCooldown = 0.5f;

        private float m_AnimationTimerNA = 0;
        private bool m_AtNormalAttack;

        private float m_AnimationTimerCA = 0;
        [SerializeField] private float m_ChargingTimerCA = 0;
        private bool m_BeforeChargedAttack;
        private bool m_AtChargedAttack;

        private float timerE = 0;
        private bool m_AtAnimationE;
        
        private float m_AnalSphincterTimer = 0;
        private bool m_AtSpray = false;

        private bool m_ToNormalAttack;
        

        private GRDPlayer player;
        private SkillState m_CurrentSkill;

        public float SprayChargeAmount => m_AnalSphincterTimer / m_SprayTime;

        //[SerializeField] private Animator m_Animator;

        new Rigidbody rigidbody;

        public event Action OnTakeSprayStance;
        public event Action OnEndSprayStance;

        [SerializeField] private Turret m_NormalTurret;
        [SerializeField] private HealSide m_HealingSide;
        public void AssignTurretCamera(Camera cam)
        {
            if (m_NormalTurret)
                m_NormalTurret.SetCamera(cam);
            if (m_Doll.AnusNipplesTurret)
                m_Doll.AnusNipplesTurret.SetCamera(cam);
        }

     //   [SerializeField] private Doll m_Doll; 
        
        public event Action<float> OnUpdateCooldownTime;
        public Action<float> UpdateCooldown(float time)
        {
            return (time) =>
            {
                Dashboard.Instance.UpdateCooldown(time);
            };
        }
        private void Awake()
        {
          //  m_CurrentDoll = transform.parent.GetComponent<Doll>();
        }
       
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = Player.Instance as GRDPlayer;
  //          m_Doll = player.ActiveDoll;

            
            rigidbody = gameObject. transform.parent.GetComponent<Rigidbody>();
            OnUpdateCooldownTime += UpdateCooldown(m_Cooldown);
            
        }
        private bool _;

        public bool IsAnimationPlaying(string animationName)
        {
            // ����� ���������� � ���������
            var animatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            // �������, ���� �� � ��� ��� �����-�� ��������, �� ���������� true
            if (animatorStateInfo.IsName(animationName))
                return true;

            return false;
        }
        public float AnimationNormalizedTime(string animationTag)
        {
            // ����� ���������� � ���������
            var animatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            // �������, ���� �� � ��� ��� �����-�� ��������, �� ���������� true
            if (animatorStateInfo.IsTag(animationTag))
                return animatorStateInfo.normalizedTime;
            return 0f;
        }


        int m_CenterOfMassIndex = 0;
        



        // Update is called once per frame
        void Update()
        {

            if (AnimationNormalizedTime("ls") >= 1.0f)
            {
                Idle();
            }


            if (AnimationNormalizedTime(".") >= 1.0f)
            {
                if (m_LesserSkillBuff)
                    LesserSkIdle();
                else
                    Idle();
            }
            if (AnimationNormalizedTime("-") >= 1.0f)
            {
                if (m_LesserSkillBuff)
                    LesserSkIdle();
                else
                    Idle();
            }

            if (m_BeforeChargedAttack)
            {
                m_ChargingTimerCA += Time.deltaTime;


                if (m_ChargingTimerCA >= m_ChargedAttackTime)
                {
                    m_BeforeChargedAttack = false;  
                    print("Charged");
                    ChargedAttack();
                }
            }

            if (m_AtSpray)
            {
                m_AnalSphincterTimer += Time.deltaTime;
                // ���� ����� ������
                if (m_AnalSphincterTimer >= m_SprayTime)
                {
                    EndGreaterSkill();
                }
            }

         
            if (m_Doll)
            {

            }



            
        }

        public void Idle()
        {
            m_Doll.DollController.SetIdle();
            m_CenterOfMassIndex = 0;


            m_Animator.SetBool("TailUp", false);
        }
        public void LesserSkIdle()
        {
            m_Animator.SetInteger("Autom", 15);
            m_CenterOfMassIndex = 0;
        }

        IEnumerator WaitForCooldown()
        {
            yield return new WaitForSeconds(m_AttackCooldown);
        }

        bool attackAtCooldown = false;
        public void StartAttack()
        {
            if (attackAtCooldown) return;

            m_ChargingTimerCA = 0;
            m_BeforeChargedAttack = true;

            if (m_Doll.DollID != 2 || !m_LesserSkillBuff)

                m_Animator.SetBool("TailUp", true);

            
            StartCoroutine(ChargedAttackTime());

            StartCoroutine(WaitForCooldown());

            IEnumerator WaitForCooldown()
            {
                attackAtCooldown = true;
                yield return new WaitForSeconds(m_AttackCooldown);
                attackAtCooldown = false;


            }

        }

        public void EndAttack()
        {
            // ���� ������� �����
            if (m_ChargingTimerCA < m_ChargedAttackTime)
            {
                NormalAttack();
            }
            // ���� ���������� - ���������
            else
            {
                Idle();
            }

            m_ChargingTimerCA = 0;
            m_BeforeChargedAttack = false;

            m_AtChargedAttack = false;

            

        }
        public void EnterSprayMode()
        {
            m_Doll.State = 4;
            m_Animator.SetInteger("Autom", 4);
            m_Animator.SetBool("TailUp", true);
            ShipInputController.mouseTorque = false;
            print(OnTakeSprayStance != null);
            OnTakeSprayStance.Invoke();
        }

        public void ExitSprayMode()
        {
            m_Doll.State = 0;
            m_Animator.SetInteger("Autom", 0);
            m_Animator.SetBool("TailUp", false);
            ShipInputController.mouseTorque = true;

            OnEndSprayStance.Invoke();
        }



        private enum SkillState
        {   None,
            NormalAttack,
            ChargedAttack,
            LesserSkill,         
            GreaterSkill
        }


        private void NormalAttack()
        {
            m_Animator.SetInteger("Autom", 7);
            m_CenterOfMassIndex = 1;
            m_AtNormalAttack = true;
            m_Doll.Sounds[1].Play();


            if (m_Doll.DollID == 1)
                m_NormalTurret?.Fire();

            if (m_LesserSkillBuff)
            {
                if (m_Doll.DollID == 2)
                {
                    m_Animator.SetInteger("Autom", 13);
                }
            }

            StartCoroutine(OffTime(0.5f));
        }

        IEnumerator ChargedAttackTime()
        {
            yield return new WaitForSeconds(m_ChargingTimerCA);

            
        }
        IEnumerator OffTime(float time)
        {
            yield return new WaitForSeconds(time);


            if (m_AtChargedAttack)
            {
                if (m_LesserSkillBuff)
                {
                    LesserSkIdle();
                }
                else
                {
                    Idle();
                }
            }
        }
        private void ChargedAttack()
        {
            

            m_Animator.SetInteger("Autom", 8);
            m_CenterOfMassIndex = 1;


            if (m_Doll.DollID == 1 && !m_AtChargedAttack)
            {
                m_NormalTurret.AssignLoadout(m_CATurretProps);
                m_NormalTurret.SetProjProps(m_AlternativeProjectileProps);

                m_NormalTurret?.Fire();


                m_NormalTurret.AssignLoadout(m_TurretProps);
                m_NormalTurret.SetProjProps(m_ProjectileProps);
            }

            if (m_LesserSkillBuff)
            {
                if (m_Doll.DollID == 2)
                {
                    m_Animator.SetInteger("Autom", 14);
                }
            }

            m_AtChargedAttack = true;
            m_Doll.Sounds[2].Play();


            StartCoroutine(OffTime(m_ChargedAttackAnimationTime));
        }




        //        public Action<Doll> Link()
        //        {
        //            return (Doll d) =>
        //            {
        ////                m_CurrentDoll = d;
        //            };
        //        }

        //        public void SetDoll(Doll doll)
        //        {
        // //           m_CurrentDoll = doll;
        //        }

        [SerializeField] private int attackDamage;
        public int AttackDamage => attackDamage;

       [SerializeField]  private bool m_FlehmenCooldown;
        public bool FlehmenCooldown => m_FlehmenCooldown;

        public void SetFlehmenCooldown() 
        {
            m_FlehmenCooldown = true;
        }
        [SerializeField] private float m_Cooldown;
        public float Cooldown => m_Cooldown;

        public enum AttackType
        {
            Melee,
            Ranged
        }

        [SerializeField] private int m_HealAmount = 336;
        [SerializeField] private int m_AttackPower = 288;
        //       public event Action<bool> OnFlehmenCooldown;

        [SerializeField] private ProjectileProperties m_ProjectileProps;
        [SerializeField] private ProjectileProperties m_AlternativeProjectileProps;
        [SerializeField] private TurretProperties m_TurretProps;
        [SerializeField] private TurretProperties m_CATurretProps;
        [SerializeField] private TurretProperties m_ETurretProps;

        [SerializeField] private bool m_LesserSkillBuff;
        public bool LesserSkillBuff => m_LesserSkillBuff;

        [SerializeField] private float m_BuffDuration = 10;


      
        public void LesserSkill()
        {


            print("Lesser"); 

            IEnumerator FlehmenCDSkill()
            {
                print("Flehmen at CD");

                m_FlehmenCooldown = true;
                for (int i = 0; i < m_Cooldown; i++)
                {
                    OnUpdateCooldownTime(m_Cooldown - i);
                    yield return new WaitForSeconds(1);
                }
                m_FlehmenCooldown = false;

                Dashboard.Instance.Btn();

                print("Flehmen free");
            }
            ;



            print(m_FlehmenCooldown);
            if (!m_FlehmenCooldown)
            {
               // m_Doll.LesserSkill();
                m_AtAnimationE = true;
                if (m_Doll.DollID == 0)
                {
                    m_Animator.SetInteger("Autom", 9);
                }
                if (m_Doll.DollID == 1 || m_Doll.DollID == 2)
                {
                    m_Animator.SetInteger("Autom", 15);
                }

                if (m_Doll.DollID == 0)
                {
                    // Party.Instance.RestoreHPAll(m_HealAmosunt);
                    //   m_NormalTurret.Fire();

                    m_Animator.SetBool("FX", true);
                    m_HealingSide.HealParty();
                    StartCoroutine(EffectTimer());
                }
                if (m_Doll.DollID == 1)
                {
                    m_NormalTurret.AssignLoadout(m_ETurretProps);
                    m_LesserSkillBuff = true;
                    StartCoroutine(EffectTimer());
                }
                if (m_Doll.DollID == 2)
                {
                    m_LesserSkillBuff = true;
                    StartCoroutine(EffectTimer());
                }

                StartCoroutine(FlehmenCDSkill());
            }

           

            IEnumerator EffectTimer()
            {
                yield return new WaitForSeconds(m_BuffDuration);

                if (m_Doll.DollID == 0)
                {
                    m_Animator.SetBool("FX", false);
                }

                if (m_Doll.DollID == 1)
                {
                    m_NormalTurret.AssignLoadout(m_TurretProps);
                    m_LesserSkillBuff = false;
                    Idle();
                }
                if (m_Doll.DollID == 2)
                {
                    m_LesserSkillBuff = false;
                    Idle();
                }


            }

        }

        /// <summary>
        /// ���������� ������� �������� ����� (�����), �������� ��������� ���������
        /// </summary>
        public void StartGreaterSkill()
        {
            m_AtSpray = true;
            m_AnalSphincterTimer = 0;

        }

        //public void ContinueGreaterSkill()
        //{
        //    if (m_AnalSphincterTimer > m_SprayTime)
        //    {
        //        EndGreaterSkill();
        //    }
        //}
        
        /// <summary>
        /// ������� ������, ���� ����� ������ ������
        /// ����� - ������.
        /// </summary>
        public void EndGreaterSkill()
        {
            // ����� ������
            if (m_AnalSphincterTimer > m_SprayTime)
            {
                //m_Doll?.Spray();

                m_Doll.CareToiletStat(ToiletStat.AnalSpray, m_Doll.AnalGlandVolume / 5);


                m_Doll.Sounds[6].Play();
                m_Doll.Sounds[UnityEngine.Random.Range(7, 9)].Play();
                m_Doll.AnusNipplesTurret.Fire();

            }
            m_AnalSphincterTimer = 0;
            m_AtSpray = false;


        }
        public void CancelGreaterSkill()
        {
            m_AtSpray = false;
            m_AnalSphincterTimer = 0;

        }


    }
}

