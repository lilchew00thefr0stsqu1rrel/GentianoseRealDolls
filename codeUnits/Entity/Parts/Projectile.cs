using Common;
using UnityEngine;
using NTC.Pool;
using SpaceShooter;

//namespace SpaceShooter
namespace GentianoseRealDolls
{
    public class Projectile : ProjectileBase
    {

        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        [SerializeField] private SpriteRenderer m_SpriteRend;  // cache



        [SerializeField] private AttackSide m_AoE;
        [SerializeField] private AttackSide m_SecondaryAoE;
        public void SetAoEParent(Destructible destructible)
        {
            if (!m_AoE || !m_SecondaryAoE) return;
            m_AoE.SetParent(destructible);
            m_SecondaryAoE.SetParent(destructible);
        }

        protected override void ApplyChildProps()
        {
            m_SpriteRend = GetComponentInChildren<SpriteRenderer>();
            if (m_SpriteRend != null)
            {
                m_SpriteRend.sprite = m_Properties.VisualSprite;
            }
        }





        protected override void OnHit(DestructibleBase destructibleBase)
        {
            
            //if (m_Parent == Player.Instance.ActiveShip)
            //{
               
            //    Player.Instance.AddScore(((Destructible) destructibleBase).ScoreValue);

            //    if (destructibleBase is SpaceShip)
            //    {
            //        if (destructibleBase.HitPoints <= 0)
            //            Player.Instance.AddKill();


            //    }
                
            //}
        }

        protected override void OnProjectileLifeEnd(Collider col, Vector3 pos)
        {


            if (m_ImpactEffectPrefab != null)
            {


                ImpactEffect impact = Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);
                impact.transform.forward = col.transform.forward;
                //~~ ** ~~

                Explosion explosion = impact.GetComponent<Explosion>();

                if (explosion != null)
                {
                    explosion.SetSourceShip((Destructible)m_Parent);
                    explosion.SetDamage(m_Damage);
                }
            }


            print("disapp");


          //  Destroy(gameObject, 0);
              NightPool.Despawn(gameObject, 0);
            
        }

       

        protected override void OnProjectileLifeEnd()
        {

            if (m_ImpactEffectPrefab != null)
            {
                

                ImpactEffect impact = Instantiate(m_ImpactEffectPrefab, gameObject.transform.position, Quaternion.identity);
                impact.transform.forward = gameObject.transform.forward;
                //~~ ** ~~

                Explosion explosion = impact.GetComponent<Explosion>();

                if (explosion != null)
                {
                    explosion.SetSourceShip((Destructible)m_Parent);
                    explosion.SetDamage(m_Damage);
                }
            }

            Destroy(gameObject, 0);


        }

        private void OnTriggerEnter(Collider other)
        {   if (m_ImpactEffectPrefab != null)
                Instantiate(m_ImpactEffectPrefab, transform.position + transform.forward, transform.rotation);
        }

 

    } 


}

