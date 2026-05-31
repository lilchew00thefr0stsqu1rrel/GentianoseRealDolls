using UnityEngine;
using Common;

namespace GentianoseRealDolls
{
    public class Explosion : ImpactEffect
    {
        private int m_Damage;
        private Destructible m_SourceShip;

        public void SetSourceShip(Destructible ship)
        {
            m_SourceShip = ship;
        }

        public void SetDamage(int damage)
        {
            m_Damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            

            if (m_SourceShip != null && collision.transform.root == m_SourceShip.transform) return;

            Destructible dest = collision.transform.root.GetComponent<Destructible>();
            if (dest != null) 
            {

                dest.ApplyDamage(m_Damage);
                
                OnHit(dest);
            }
            
        }

        private void OnHit(DestructibleBase destructibleBase)
        {

            if (m_SourceShip == Player.Instance.ActiveShip)
            {

                Player.Instance.AddScore(((Destructible)destructibleBase).ScoreValue);

                if (destructibleBase is SpaceShip)
                {
                    if (destructibleBase.HitPoints <= 0)
                        Player.Instance.AddKill();
                }

            }
        }
    }
}

