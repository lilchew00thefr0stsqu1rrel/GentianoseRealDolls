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
        [Header("Doll parts")]

        [SerializeField] private DollPart m_AttackPart;
        [SerializeField] private DollPart m_LesserSkillPart;
        [SerializeField] private Turret m_AnusTurret;

        [Header("Params")]

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



        [SerializeField] private bool m_IsSprayStance;
        Vector2 m_AimInput;

        [SerializeField] private int attackDamage;
        public int AttackDamage => attackDamage;

        [SerializeField] private bool m_FlehmenCooldown;
        public bool FlehmenCooldown => m_FlehmenCooldown;

        public void SetFlehmenCooldown()
        {
            m_FlehmenCooldown = true;
        }
        [SerializeField] private float m_Cooldown;
        public float Cooldown => m_Cooldown;
        public void SetAimInput(Vector2 aimInput)
        {
            m_AimInput = aimInput;
            m_AnusTurret.SetAimInput(aimInput);
            m_AttackPart.SetAimInput(aimInput);
        }


        public void AssignTurretCamera(Camera cam)
        {
            if (m_AttackPart is Turret)
                (m_AttackPart as Turret).SetCamera(cam);
            if (m_LesserSkillPart is Turret)
                (m_LesserSkillPart as Turret).SetCamera(cam);
            if (m_AnusTurret)
                m_Doll.AnusNipplesTurret.SetCamera(cam);
        }

     //   [SerializeField] private Doll m_Doll; 
        
        public event Action<float> OnUpdateCooldownTime;
        public Action<float> UpdateCooldown(float time)
        {
            return (time) =>
            {
                m_Dashboard.UpdateCooldown(time);
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

        public float AnimationNormalizedTime(string animationTag)
        {
            if (!m_AnimatorGuard) return 0;

            if (m_AnimatorGuard.IsTag(animationTag))
                return m_AnimatorGuard.NormalizedTime();
            return 0f;
        }


        int m_CenterOfMassIndex = 0;
        
        


        // Update is called once per frame
       protected override void Run()
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
                    ChargedAttack(m_AimInput);
                }
            }

            if (m_AtSpray)
            {
                m_AnalSphincterTimer += Time.deltaTime;
                // ���� ����� ������
                if (m_AnalSphincterTimer >= m_SprayTime)
                {
                    EndGreaterSkill(m_AimInput);
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


            //m_Animator.SetBool("TailUp", false);
        }
        public void LesserSkIdle()
        {
            // m_Animator.SetInteger("Autom", 15);
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
                StartCoroutine(ChargedAttackTime());

            StartCoroutine(WaitForCooldown());

            IEnumerator WaitForCooldown()
            {
                attackAtCooldown = true;
                yield return new WaitForSeconds(m_AttackCooldown);
                attackAtCooldown = false;
            }

        }

        public void EndAttack(Vector2 aimInput)
        {
            if (m_ChargingTimerCA < m_ChargedAttackTime)
            {
                NormalAttack(aimInput);
            }
            else
            {
                Idle();
            }
            m_ChargingTimerCA = 0;
            m_BeforeChargedAttack = false;
            m_AtChargedAttack = false;
        }

        private const int SprayStanceID = 4;

        public void SprayModeOnOff()
        {
            if (m_IsSprayStance)
            {
                m_Doll.State = 0;
                print("U-like" + m_AnimatorGuard + " " + m_IsSprayStance);
                m_AnimatorGuard.SetAnimation(0);
                //m_Animator.SetBool("TailUp", false);
                MoveInputController.mouseTorque = true;
            }
            else
            {
                m_Doll.State = 4;

                print("U-like" + m_AnimatorGuard + " " + m_IsSprayStance);
                m_AnimatorGuard.SetAnimation(SprayStanceID);
                //m_Animator.SetBool("TailUp", true);
                MoveInputController.mouseTorque = false;
            }
            m_IsSprayStance = !m_IsSprayStance;
        }

        public void EnterSprayMode()
        {
            m_Doll.State = 4;
            m_AnimatorGuard.SetAnimation(SprayStanceID);
            //m_Animator.SetBool("TailUp", true);
            MoveInputController.mouseTorque = false;
            //print(OnTakeSprayStance != null);
           // OnTakeSprayStance.Invoke();
        }

        public void ExitSprayMode()
        {
            m_Doll.State = 0;
            m_AnimatorGuard.SetAnimation(0);
            m_Doll.DollController.SetIdle();
            //m_Animator.SetBool("TailUp", false);
            MoveInputController.mouseTorque = true;

            //OnEndSprayStance.Invoke();
        }



        private enum SkillState
        {   None,
            NormalAttack,
            ChargedAttack,
            LesserSkill,         
            GreaterSkill
        }

        private void NormalAttack(Vector2 aimInput)
        {
            m_AnimatorGuard.SetAnimation(7);
            m_CenterOfMassIndex = 1;
            m_AtNormalAttack = true;
            m_Doll.Sounds[1].Play();


            m_AttackPart.Use(aimInput);

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
        private void ChargedAttack(Vector2 aimInput)
        {


            m_AnimatorGuard.SetAnimation(8);
            m_CenterOfMassIndex = 1;


            (m_AttackPart as Turret).AssignLoadout(m_CATurretProps);
            (m_AttackPart as Turret).SetProjProps(m_AlternativeProjectileProps);
            m_AttackPart.Use(aimInput);
            (m_AttackPart as Turret).AssignLoadout(m_TurretProps);
            (m_AttackPart as Turret).SetProjProps(m_ProjectileProps);


            m_AnimatorGuard.SetAnimation(14);

            ///
           

            m_AtChargedAttack = true;
            m_Doll.Sounds[2].Play();


            StartCoroutine(OffTime(m_ChargedAttackAnimationTime));
        }


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


            if (!m_FlehmenCooldown)
            {
                m_AtAnimationE = true;
                m_AnimatorGuard.SetAnimation(9);
                m_LesserSkillPart.Use(m_AimInput);
                StartCoroutine(EffectTimer());
                m_LesserSkillBuff = true;
                StartCoroutine(FlehmenCDSkill());
            }

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
                m_Dashboard.Btn();
                print("Flehmen free");
            }

            IEnumerator EffectTimer()
            {
                yield return new WaitForSeconds(m_BuffDuration);
               
                m_LesserSkillBuff = false;
                Idle();
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
         
        public void EndGreaterSkill(Vector2 aimInput)
        {
            // ����� ������
            if (m_AnalSphincterTimer > m_SprayTime)
            {
                m_Doll.CareToiletStat(ToiletStat.AnalSpray, m_Doll.AnalGlandVolume / 5);


                m_Doll.Sounds[6].Play();
                m_Doll.Sounds[UnityEngine.Random.Range(7, 9)].Play();

                m_Doll.AnusNipplesTurret.SetCamera(Camera.main);
                m_Doll.AnusNipplesTurret.Fire(aimInput);
            }
            m_AnalSphincterTimer = 0;
            m_AtSpray = false;
        }

    }
}

