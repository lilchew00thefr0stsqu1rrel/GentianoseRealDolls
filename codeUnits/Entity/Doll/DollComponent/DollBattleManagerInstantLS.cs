using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

using Cysharp.Threading.Tasks;
using System;

namespace GentianoseRealDolls
{

    public class DollBattleManagerInstantLS : DollBattleManager
    {
        // Callimico
        [SerializeField] private Vector3 m_Blink;
        [SerializeField] private float m_BlinkTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override async UniTask LesserSkill()
        {
            print("Lesser");

            if (m_AnimatorGuard == null) m_AnimatorGuard = GetComponent<AnimatorGuard>();

            if (!m_FlehmenCooldown)
            {
                m_AtAnimationE = true;

                m_AnimatorGuard.SetAnimation(9);


                //await Task.Delay((int)(1000 * m_BlinkTime));
                //StartCoroutine(BlinkTimer());


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

                //StartCoroutine(EffectTimer());

                m_LesserSkillBuff = true;

                //StartCoroutine(FlehmenCDSkill());
                m_LesserSkillCooldownTime = m_Cooldown;



                await UniTask.Delay(TimeSpan.FromSeconds(m_BlinkTime));

                transform.parent.position += m_Blink.z * transform.parent.forward;
                transform.parent.position += m_Blink.y * transform.parent.up;

                m_LesserSkillPart?.SetActionTime(2);
                m_LesserSkillPart?.Use();

                await UniTask.Delay(TimeSpan.FromSeconds(m_BuffDuration));

                m_LesserSkillBuff = false;
                m_LesserSkillGaitBuff = false;

                if (m_EffectSign != null)
                    m_EffectSign.SetActive(false);
                m_AnimatorGuard.SetAnimation(0);

            }

            //IEnumerator FlehmenCDSkill()
            //{
            //    print("Flehmen at CD");
            //    m_FlehmenCooldown = true;
            //    for (int i = 0; i < m_Cooldown; i++)
            //    {
            //        //OnUpdateCooldownTime(m_Cooldown - i);
            //        m_LesserSkillCooldownTime--;
            //        yield return new WaitForSeconds(1);
            //    }
            //    m_FlehmenCooldown = false;
            //    //  m_Dashboard.Btn();
            //    print("Flehmen free");
            //}

            
                
            

        }
    }
}
