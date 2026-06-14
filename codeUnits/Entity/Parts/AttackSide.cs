using GentianoseRealDolls;
using NTC.Pool;
using SpaceShooter;
using System.Threading.Tasks;
using UnityEngine;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(BoxCollider))]
    public class AttackSide : DollPart
    {

        [SerializeField] private Collider m_AttackTrigger;
        private int millisecondsInSecond = 1000;
        private void Awake()
        {
            m_AttackTrigger = GetComponent<Collider>();
            m_AttackTrigger.enabled = false;


            if (m_Perm)
            {
                m_AttackTrigger.enabled = true;
            }
        }

        

        public override void Use()
        {
            HitboxTimer(m_Time);
        }

        private async void HitboxTimer(float time)
        {
            m_AttackTrigger.enabled = true;
            await Task.Delay((int)(time * millisecondsInSecond));
            m_AttackTrigger.enabled = false;
        }


        [SerializeField] private bool m_SprayType;

        [SerializeField] private int m_StatusID;

        [SerializeField] private int m_AttackDamage;

        [SerializeField] private float m_CooldownDuration = 0.1f;

        [SerializeField] private bool m_BelongsToDoll;

        // 0 - питомцы
        // 6 - враги
        // 138 - неуязвимые существа

        [SerializeField] private int m_TeamID;

        private float m_Time;

        private bool m_Cooldown;
        // Update is called once per frame
        void Update()
        {
            if (m_Cooldown)
            {
                m_Time += Time.deltaTime;
                if (m_Time > m_CooldownDuration)
                {
                    m_Cooldown = false;
                    m_Time = 0;
                }
            }
        }

        public override void SetActionTime(float time)
        {
            m_Time = time;
        }

        public void SetDamage(int damage)
        {
            m_AttackDamage = damage;
        }

        public void SetParent(Destructible destructible)
        {
            parent = destructible;
        }

        [SerializeField] private Destructible parent;

        [SerializeField] private float m_Multiplier = 1;
        [SerializeField] private bool m_Perm;
 
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                Destructible dest = other.GetComponent<Destructible>();

                if (dest != null)
                {
                    if (m_EffectBale)
                    {
                        var point = other.ClosestPoint(transform.position);
                        m_EffectBale.SetActive(true);
                        m_EffectBale.transform.position = point;
                    }

                    if (dest.TeamId != m_TeamID)
                    {
                        if (!m_Cooldown)
                        {

                            if (m_SprayType)
                            {
                                dest.ApplyDamageOverTime(m_AttackDamage, 14);
                                m_Cooldown = true;
                            }
                            else
                            {
                                dest.ApplyDamage(m_AttackDamage);
                                m_Cooldown = true;
                            }
                        }
                        
                    }
                }

            }
        }
    }
}

