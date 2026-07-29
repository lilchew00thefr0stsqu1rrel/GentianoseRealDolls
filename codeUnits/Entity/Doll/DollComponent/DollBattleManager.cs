using Common;
using SpaceShooter;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
        public enum AttackType
        {
            Melee,
            Ranged
        }

        [Header("Doll parts")]

        [SerializeField] private DollPart m_NormalAttackPart;
        [SerializeField] private DollPart m_LesserSkillNormalAttackPart;
        [SerializeField] private DollPart m_ChargedAttackPart;
        [SerializeField] private DollPart m_LesserSkillChargedAttackPart;
        [SerializeField] private DollPart m_LesserSkillPart;
        [SerializeField] private Turret m_AnusTurret;

        [SerializeField] private DollPart m_SummonPrefab; //14vi2026
        [SerializeField] private OffField m_OffField;
        [SerializeField] private GameObject m_EffectSign;

        [Header("***")]

        private GRDTimer chargedTimer;
        [SerializeField] private float m_ChargedAttackTime = 0.592f;
        [SerializeField] private float m_AttackTime = 0.2f;
        [SerializeField] private float m_SprayTime = 0.851f;
        [SerializeField] private float m_AttackCooldown = 0.5f;

        [SerializeField] private int m_SprayChargeSteps;
        public float SprayChargeSteps => m_SprayChargeSteps;

        [SerializeField] private int m_SprayPortionsInScentSacs = 5;

        [SerializeField] private int m_FireIterAmount = 5;
        [SerializeField] int countAtt = 0;
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
        private bool m_ReadySpray = false;


        public float SprayChargeAmount => m_AnalSphincterTimer / m_SprayTime;  ///  

        public event Action OnTakeSprayStance;
        public event Action OnEndSprayStance;



        [SerializeField] private bool m_IsSprayStance;
        Vector2 m_AimInput;

        [SerializeField] private int attackDamage;
        public int AttackDamage => attackDamage;

        [SerializeField] private bool m_FlehmenCooldown;
        public bool FlehmenCooldown => m_FlehmenCooldown;
        [SerializeField] private int m_HealAmount = 336;
        [SerializeField] private int m_AttackPower = 288;

        [SerializeField] private bool m_LesserSkillBuff;

        // 18 VII '26
        [SerializeField] private bool m_LesserSkillGaitBuff;
        public bool LesserSkillGaitBuff => m_LesserSkillGaitBuff;


        [SerializeField] private float m_BuffDuration = 10;


        private float m_LesserSkillCooldownTime;
        public float LesserSkillCooldownTime => m_LesserSkillCooldownTime;

        public void SetFlehmenCooldown()
        {
            m_FlehmenCooldown = true;
        }
        [SerializeField] private float m_Cooldown;
        public float Cooldown => m_Cooldown;

        public bool SprayStanceOn => m_IsSprayStance;

        Vector3 posDollStart;

        // Cheirogaleus
        [SerializeField] private float m_UrineTrailMinDist = 0.5f; 
        [SerializeField] private float m_UrineTrailTime = 5;

        // Callimico
        [SerializeField] private Vector3 m_Blink;
        [SerializeField] private float m_BlinkTime;

        public void SetOffField(OffField off)
        {
            m_OffField = off;
        }

        public void SetAimInput(Vector2 aimInput)
        {
            m_AimInput = aimInput;
            m_AnusTurret.SetAimInput(aimInput);
            m_NormalAttackPart.SetAimInput(aimInput);
        }

        public void AssignTurretCamera(Camera cam)
        {
            if (m_NormalAttackPart is Turret)
                (m_NormalAttackPart as Turret).SetCamera(cam);

            if (m_ChargedAttackPart is Turret)
                (m_ChargedAttackPart as Turret).SetCamera(cam);

            if (m_LesserSkillNormalAttackPart is Turret)
                (m_LesserSkillNormalAttackPart as Turret).SetCamera(cam);

            if (m_LesserSkillChargedAttackPart is Turret)
                (m_LesserSkillChargedAttackPart as Turret).SetCamera(cam);

            if (m_LesserSkillPart is Turret)
                (m_LesserSkillPart as Turret).SetCamera(cam);

            if (m_AnusTurret)
                m_Doll.AnusNipplesTurret.SetCamera(cam);
        }
        
        public event Action<float> OnUpdateCooldownTime;
        public Action<float> UpdateCooldown(float time)
        {
            return (time) =>
            {
                m_Dashboard.UpdateCooldown(time);
            };
        }

        public override void ConstructDollCom(Party party)
        {
            base.ConstructDollCom(party);
            if (m_LesserSkillPart is HealSide)
            {
                (m_LesserSkillPart as HealSide).SetParty(party);
            }
        }



        private void Awake()
        {
        }
       
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            OnUpdateCooldownTime += UpdateCooldown(m_Cooldown);
        }
        private bool _;

        public float AnimationNormalizedTime(string animationTag)
        {
            if (!m_AnimatorGuard)
            {
                print("NO ANIMATion GUard");
                return 0;
            }

            if (m_AnimatorGuard.IsTag(animationTag))
                return m_AnimatorGuard.NormalizedTime();
            return 0f;
        }

        // Update is called once per frame
        protected override void Run()
        {

            if (m_BeforeChargedAttack)
            {
                m_ChargingTimerCA += Time.deltaTime;

                if (m_ChargingTimerCA >= m_ChargedAttackTime)
                {
                    print("Charged");
                    ChargedAttack(m_AimInput);
                }
            }

        }

        public void Idle()
        {
            if (m_LesserSkillBuff)
            {
                m_AnimatorGuard.SetAnimation(9);
            }
            else
            {
                m_AnimatorGuard.SetAnimation(0);
            }
        }

        #region Normal Attack

        bool attackAtCooldown = false;

        public async void StartAttack()
        {
            print("StartAtt");
            gameObject.SetActive(true);

            if (attackAtCooldown) return;

            m_ChargingTimerCA = 0;
            m_BeforeChargedAttack = true;

            attackAtCooldown = true;
            await Task.Delay((int)(m_AttackCooldown * 1000));
            attackAtCooldown = false;
        }

        public void EndAttack(Vector2 aimInput)
        {
            print("EndAtt");
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


            m_Party.Camera.OffAimMode(m_Doll.DollSize);
        }

        private const int SprayStanceID = 4;
        private const int NormalAttackID = 7;
        private const int ChargedAttackID = 8;
        private const int ModifiedNormalAttackID = 13;
        private const int ModifiedChargedAttackID = 14;

        private enum SkillState
        {   None,
            NormalAttack,
            ChargedAttack,
            LesserSkill,         
            GreaterSkill
        }

        private async void NormalAttack(Vector2 aimInput)
        {
            if (m_AnimatorGuard == null) m_AnimatorGuard = GetComponent<AnimatorGuard>();


            //20vii26
            m_OffField.UseParfusion();

            float ntime = m_AnimatorGuard.GetAnimationLength($"{10000 + m_Doll.DollID}0007");
            float lntime = m_AnimatorGuard.GetAnimationLength($"{10000 + m_Doll.DollID}0013");

            if (m_LesserSkillBuff)
            {
                m_AnimatorGuard.SetAnimation(13);
                m_LesserSkillNormalAttackPart.SetAimInput(aimInput);
                m_LesserSkillNormalAttackPart.SetActionTime(lntime);
                m_LesserSkillNormalAttackPart.Use();


                await Task.Delay((int)(lntime * 1000));
                m_AnimatorGuard.SetAnimation(0);
            }
            else
            {
                m_AnimatorGuard.SetAnimation(7); 
                m_LesserSkillNormalAttackPart.SetAimInput(aimInput);
                m_LesserSkillNormalAttackPart.SetActionTime(ntime);
                m_NormalAttackPart.Use();

                await Task.Delay((int)(ntime * 1000));
                m_AnimatorGuard.SetAnimation(0);

            }
            m_AtNormalAttack = true;
            m_Doll?.Sounds[1].Play();

        }



        private async void ChargedAttack(Vector2 aimInput)
        {
            if (m_AnimatorGuard == null) m_AnimatorGuard = GetComponent<AnimatorGuard>();

            //20vii26
            m_OffField.UseParfusion();

            float ctime = m_AnimatorGuard.GetAnimationLength($"{10000 + m_Doll.DollID}0008");
            float lctime = m_AnimatorGuard.GetAnimationLength($"{10000 + m_Doll.DollID}0014");


            if (m_LesserSkillBuff)
            {
                if (m_LesserSkillChargedAttackPart is Turret && (m_LesserSkillChargedAttackPart as Turret).Mode == TurretMode.Thorn)
                {
                    m_Party.Camera.AimMode();
                }

                m_AnimatorGuard.SetAnimation(14);
                m_LesserSkillChargedAttackPart.SetAimInput(aimInput);
                m_LesserSkillChargedAttackPart.SetActionTime(lctime);
                m_LesserSkillChargedAttackPart.Use();

                if (!m_LesserSkillChargedAttackPart.Repeating)
                {
                    m_BeforeChargedAttack = false;
                }

                await Task.Delay((int)(lctime * 1000));

                if (m_ChargedAttackPart.Repeating)
                {
                    if (countAtt < m_FireIterAmount)
                        countAtt++;
                    else
                    {
                        countAtt = 0;
                        m_BeforeChargedAttack = false;
                    }
                }
                //m_Party.Camera.OffAimMode();

                Idle();
            }
            else
            {
                if (m_ChargedAttackPart is Turret && (m_ChargedAttackPart as Turret).Mode == TurretMode.Thorn)
                {
                    m_Party.Camera.AimMode();
                }

                m_AnimatorGuard.SetAnimation(8);
                m_ChargedAttackPart.SetAimInput(aimInput);
                m_ChargedAttackPart.SetActionTime(ctime);
                m_ChargedAttackPart.Use();



                if (!m_ChargedAttackPart.Repeating)
                {
                    m_BeforeChargedAttack = false;
                }

                await Task.Delay((int)(ctime * 1000));

                if (m_ChargedAttackPart.Repeating)
                {
                    if (countAtt < m_FireIterAmount)
                        countAtt++;
                    else
                    {
                        countAtt = 0;
                        m_BeforeChargedAttack = false;
                    }
                }

                Idle();
            }


            if (!m_Doll.Sounds[2].isPlaying)
                m_Doll.Sounds[2].Play();

            //m_AtChargedAttack = true;


        }

        #endregion


        public async void LesserSkill()
        {
            print("Lesser");

            if (m_AnimatorGuard == null) m_AnimatorGuard = GetComponent<AnimatorGuard>();

            if (!m_FlehmenCooldown)
            {
                m_AtAnimationE = true;

                await Task.Delay((int)(1000 * m_BlinkTime));
                transform.parent.position += m_Blink.z * transform.parent.forward;
                transform.parent.position += m_Blink.y * transform.parent.up;

                //m_AnimatorGuard.SetAnimation(9);

                //m_LesserSkillPart?.Use(m_AimInput, m_AnimatorGuard.GetAnimationLength("LesserSkill"));

                if (m_SummonPrefab && m_OffField)
                {
                    var summon = Instantiate(m_SummonPrefab, m_OffField.transform);
                    m_OffField.SetSummon(summon, m_Doll.DollID);

                    if (summon is HealSide)
                    {
                        (summon as HealSide).SetParty(m_Party);


                        summon.SetActionTime(4);
                        summon.Use();
                    }
                }

                StartCoroutine(EffectTimer());
                m_LesserSkillGaitBuff = true;

                posDollStart = transform.parent.position;
                StartCoroutine(RideTimer());

                StartCoroutine(FlehmenCDSkill());
                m_LesserSkillCooldownTime = m_Cooldown;


            }

            IEnumerator FlehmenCDSkill()
            {
                print("Flehmen at CD");
                m_FlehmenCooldown = true;
                for (int i = 0; i < m_Cooldown; i++)
                {
                    //OnUpdateCooldownTime(m_Cooldown - i);
                    m_LesserSkillCooldownTime--;
                    yield return new WaitForSeconds(1);
                }
                m_FlehmenCooldown = false;
              //  m_Dashboard.Btn();
                print("Flehmen free");
            }

            IEnumerator EffectTimer()
            {
                yield return new WaitForSeconds(m_BuffDuration);
               
                m_LesserSkillBuff = false;
                m_LesserSkillGaitBuff = false;

                if (m_EffectSign != null)
                    m_EffectSign.SetActive(false);
                m_AnimatorGuard.SetAnimation(0);
            }

            IEnumerator RideTimer()
            {
                yield return new WaitForSeconds(m_UrineTrailTime);

                if ((transform.parent.position - posDollStart).magnitude >= m_UrineTrailMinDist)
                {
                    m_LesserSkillBuff = true;
                    

                    if (m_EffectSign != null)
                    m_EffectSign.SetActive(true);
                }
            }
        }
        #region Spray
        public void SprayModeOnOff()
        {
            if (m_AnimatorGuard == null) m_AnimatorGuard = GetComponent<AnimatorGuard>();

            m_IsSprayStance = !m_IsSprayStance;

            if (m_IsSprayStance)
            {
                m_AnimatorGuard.SetAnimation(SprayStanceID);
                MoveInputController.mouseTorque = true;
            }
            else
            {
                m_AnimatorGuard.SetAnimation(0);
                MoveInputController.mouseTorque = false;
            }
        }

        /// <summary>
        /// ���������� ������� �������� ����� (�����), �������� ��������� ���������
        /// </summary>
        public void StartGreaterSkill()
        {
            m_AnalSphincterTimer = 0;
            m_AtSpray = true;

            ChargeSpray();
        }

        private async void ChargeSpray()
        {
            if (m_AtSpray)
            {
                await Task.Delay((int)(m_SprayTime * 1000 / 8));

                m_AnalSphincterTimer += m_SprayTime / 8;

                EndSpray();

                ChargeSpray();
            }
            
        }
        private void EndSpray()
        {
            if (m_AnalSphincterTimer >= m_SprayTime)
            {
                m_Doll.CareToiletStat(ToiletStat.AnalSpray, m_Doll.AnalGlandVolume / m_SprayPortionsInScentSacs);


                m_Doll.Sounds[6].Play();
                m_Doll.Sounds[UnityEngine.Random.Range(7, 9)].Play();

                m_Doll.AnusNipplesTurret.SetCamera(Camera.main);
                m_Doll.AnusNipplesTurret.Fire(m_AimInput);


                m_AnalSphincterTimer = 0;
                m_AtSpray = false;
            }

        }

        public void EndGreaterSkill()
        {
            EndSpray();

            m_AnalSphincterTimer = 0;
            m_AtSpray = false;
        }
        #endregion
    }
}


