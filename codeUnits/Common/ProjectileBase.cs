
using UnityEngine;

namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        // Для дальнейшего расширения игры.
        [Header("Properties: Scriptable Object")]
        [SerializeField] protected ProjectileProperties m_Properties;

        [Header("Properties: Values")]
        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_Lifetime;

        [SerializeField] protected int m_Damage;


        

        [SerializeField] private bool isHoming;


        [SerializeField] private float m_HomingRadius;

        [SerializeField] private float m_ExplosionRadius;

        private Vector3 m_EnemyPos;  // Gizmos

        private Vector3 m_Direction;

        private RaycastHit2D circleHit;
        private RaycastHit sphereHit;
       

        //[SerializeField] private ExplosionEffect m_ExplosionEffect;

        protected virtual void OnHit(DestructibleBase destructible) { }
        protected virtual void OnHit(Collider2D collider2D) { }
        protected virtual void OnProjectileLifeEnd(Collider col, Vector3 pos) { }

        protected virtual void OnProjectileLifeEnd() { }
        private float m_Timer;

        protected DestructibleBase m_Parent;
        private SphereCollider m_Collider;
        private DestructibleBase m_Dest;
        [Tooltip("Количество врагов, которое может задеть пронзающий или AOE снаряд перед исчезновением")]
        [SerializeField] private int m_HitAmount;  //AOE, pierce
        private int countHit;

        public void SetProperties(ProjectileProperties props)
        {
            m_Properties = props;
            
            ApplyProps();
        }

        protected virtual void ApplyProps()
        {
            m_Velocity = m_Properties.Velocity;

           // m_Collider = GetComponent<CircleCollider2D>();
            //m_Collider.radius = m_Properties.ColliderRadius;

            m_Damage = m_Properties.Damage;
            m_HitAmount = m_Properties.HitAmount;
            m_Lifetime = m_Properties.Lifetime;
            ApplyChildProps();
        }

        protected virtual void ApplyChildProps()
        {

        }

        private void Awake()
        {
            
        }


        private void Start()
        {

            

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            m_Direction = transform.forward;

            

            //m_ExplosionEffect = GetComponent<ExplosionEffect>();


        }

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;

            Vector3 step = m_Direction * stepLength;

            if (isHoming)
            {
                // Область самонаведения.
                Physics.SphereCast(transform.position - 2 * transform.up, m_HomingRadius, transform.up,
                    out sphereHit);


                //Debug.Log((bool) circleHit + " / Ayato");

                //  if (circleHit)

                if (sphereHit.collider)
                {
                    //Debug.Log(circleHit.transform.position + " / Lauma");



                    DestructibleBase destHoming = sphereHit.transform.root.GetComponent<DestructibleBase>();

                    Debug.Log((bool) destHoming + " / Dolls");

                    if (destHoming != null)
                    {
                        Debug.Log(destHoming.transform.root.name + " / Kuutar42914");
                    }

                    if (destHoming != null && destHoming.TeamId != m_Parent.TeamId)
                    {
                        // Позиция цели.
                        m_EnemyPos = destHoming.transform.position;


                        //Debug.Log(destHoming.transform.root.name + " / Kuutar42913");

                        // отрезок, соединяющий позиции снаряда и цели. 
                        Vector3 arrowToTarget = (m_EnemyPos - transform.position);



                        m_Direction = arrowToTarget.normalized;


                        Debug.Log(m_Direction + " / Nefer");



                        //destHoming.ApplyDamage(m_Damage);




                    }
                }

            }

            else  // Если не самонаводящийся снаряд
            {
                m_Timer += Time.deltaTime;

                
                

                //if (m_Collider == null || m_Collider.radius < 0.1f)
                //{
                //    Ray ray = new Ray(transform.position, transform.forward);
                //    RaycastHit[] hit = Physics.RaycastAll(ray, 0.4f);
                    

                   

                //    if (hit.Length > 0)
                //    {
                //        //if (hit.collider != m_Collider)
                //        //{
                //        //    OnHit(hit.collider);

                //        //    DestructibleBase dest = hit.collider.transform.root.GetComponent<DestructibleBase>();



                //        //    if (dest != null && dest != m_Parent)
                //        //    {

                //        //        dest.ApplyDamage(m_Damage);

                //        //        OnHit(dest);

                //        //    }

                //        //    OnProjectileLifeEnd(hit.collider, hit.point);
                //        //}
                //        //
                //        if (hit[0].rigidbody)
                //            if (hit[0].rigidbody.mass < 0.1f)
                //                return;

                       
                //    }

                //    if (m_Timer > m_Lifetime && hit.Length == 0)
                //        OnProjectileLifeEnd();


                //}

                

            }

            if (m_Timer > m_Lifetime)
                       OnProjectileLifeEnd();

            

                transform.position += new Vector3(step.x, step.y, step.z);

            
        }

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("lemur " + collision.transform.root.gameObject.name);

            if (m_Collider.radius < 0.01) return;

            DestructibleBase dest = collision.transform.parent.GetComponent<DestructibleBase>();
            if (dest)
            {
                dest.ApplyDamage(m_Damage);
                countHit++;
            }
            
            if (countHit >= m_HitAmount)
            {
                Destroy(gameObject);
            }

            if (m_Parent == null)
            {
                //BeforeDestroy();
                //Destroy(gameObject);



                  OnProjectileLifeEnd(collision, collision.transform.position);
            }

            if (m_Parent != null && collision.transform.root != m_Parent.transform)
            {
                //BeforeDestroy();
                //Destroy(gameObject);  // Уничтожить снаряд.


                  OnProjectileLifeEnd(collision, collision.transform.position);
            }
        }



      


        public void SetParentShooter(DestructibleBase parent)
        {
            m_Parent = parent;
        }

        public void SetTarget(DestructibleBase target)
        {

        }
       

        private void BeforeDestroy()
        {
           // if (m_ExplosionEffect != null)
            {
             //   m_ExplosionEffect.Explode(transform.position, m_Parent, m_Damage);
            }
          //  OnDestroy();
        }


        //private void OnDestroy()
        //{

            

        //}

        //GRD


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //if (m_EnemyPos == Vector3.zero) return;



            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position + new Vector3(0, -m_HomingRadius, 0), m_EnemyPos);



            Gizmos.color = Color.blue;
        }
#endif
    }
}

