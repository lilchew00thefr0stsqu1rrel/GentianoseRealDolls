using GentianoseRealDolls;
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
        }

        

        public override void Use(Vector2 aimInput, float attackTime)
        {
            HitboxTimer(attackTime);
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
 
        private void OnTriggerEnter(Collider other)
        {
            print("cll");
            if (parent == null) return;


            // если попадает именно коллайдер твёрдости, а не лечащего поля куклы
            if (other != null && !other.isTrigger)
            {
                Destructible dest = other.transform.root.GetComponent<Destructible>();

                if (dest != null)
                {
                    if (m_BelongsToDoll && !dest.GetComponent<Doll>() || !m_BelongsToDoll)
                    {
                        if (dest != parent)
                        {
                            if (!m_Cooldown)
                            {
                                if (m_SprayType)
                                {
                                    dest.ApplyDamage(m_AttackDamage);
                                    dest.ApplyDebuff(m_StatusID, m_Multiplier, 14);
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
}

