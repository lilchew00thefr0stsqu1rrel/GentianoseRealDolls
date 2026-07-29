using System.Collections;
using UnityEngine;

namespace GentianoseRealDolls
{

    public class DollBattleManagerGaitLS : DollBattleManager
    {
        private Vector3 posDollStart;

        // Cheirogaleus
        [SerializeField] private float m_UrineTrailMinDist = 0.5f;
        [SerializeField] private float m_UrineTrailTime = 5;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void LesserSkill()
        {
            print("Lesser");

            if (m_AnimatorGuard == null) m_AnimatorGuard = GetComponent<AnimatorGuard>();

            if (!m_FlehmenCooldown)
            {
                m_AtAnimationE = true;

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
    }
}
