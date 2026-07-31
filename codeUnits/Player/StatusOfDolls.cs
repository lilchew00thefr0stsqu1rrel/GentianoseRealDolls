using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GentianoseRealDolls
{

    public class StatusOfDolls : MonoBehaviour
    {


        [SerializeField]
        private int[] m_LesserSkillMaxTime = new int[]
        {
        3, 12, 8, 10, 3, 3, 3
        }; 
        
        [SerializeField]
        private int[] m_LesserSkillMaxCooldown = new int[]
        {
        10, 14, 16, 15, 3, 3, 6
        };

        [SerializeField]
        private int[] m_LesserSkillTimers = new int[17];

        [SerializeField]
        private int[] m_LesserSkillCooldownTimers = new int[17];
        public int[] LesserSkillCooldownTimers => m_LesserSkillCooldownTimers;

        [SerializeField]
        private bool[] m_LesserSkillBuffs = new bool[17];

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ReduceTime();
        }

        public void SetCooldown(int dollID)
        {
            m_LesserSkillCooldownTimers[dollID] = m_LesserSkillMaxCooldown[dollID];
        }

        private async void ReduceTime()
        {
            while (true)
            {
                for (int i = 0; i < 17; i++)
                {
                    if (m_LesserSkillCooldownTimers[i] > 0)
                    {
                        m_LesserSkillCooldownTimers[i]--;
                    }
                }

                await UniTask.WaitForSeconds(1);
            }
        }
    }
}
