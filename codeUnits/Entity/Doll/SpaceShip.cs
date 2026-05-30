using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using SpaceShooter;


namespace GentianoseRealDolls
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;

        [SerializeField] private Vector3 m_CenterOfMass;
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперёд сила
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;


        [SerializeField] private float m_Leap;



        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная вращательная скорость. В градусах.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;


        [SerializeField] private float m_MaxJumpVelocity;

        /// <summary>
        /// Сохранённая ссылка на ригид.
        /// </summary>
        private Rigidbody m_Rigid;

        public float MaxLinearVelocity => m_MaxLinearVelocity;

        public float MaxAngularVelocity => m_MaxAngularVelocity;

        public float MaxJumpVelocity => m_MaxJumpVelocity;
        public Sprite PreviewImage => m_PreviewImage;

        [SerializeField] private ShipStats m_Stats;
        public ShipStats Stats => m_Stats;



        [SerializeField] private Transform m_ArrowForce;

        [SerializeField] private Transform m_BackArrowForce;
        #region Public API

        /// <summary>
        /// Управление линейной тягой. -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        /// <summary>
        /// Управление прыжком. 0.0 до +1.0
        /// </summary>
        public float JumpControl { get; set; }

        //protected override void SetImpactParentShipDamage(ImpactEffect ie)
        //{
        //    ie.GameObject().GetComponent<Explosion>()?.SetSourceShip(this);
        //    ie.GetComponent<Explosion>()?.SetDamage(0);
        //}

        #endregion


        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertiaTensor = new Vector3(1, 1, 1);

            //InitOffensive();
            // Verbee
            m_CenterOfMass = new Vector3(0, -0.35f, 0.22f);
            /// <summary>
        }


        private void FixedUpdate()
        {


            UpdateRigidBody();

            //UpdateEnergyRegen();
        }

        #endregion

        [SerializeField] private SpriteRenderer testRB;

    
        [SerializeField] private Transform m_CoM;
        [SerializeField] private Transform m_Nose;

        private bool m_AtLeap;
        private bool m_OnClimbing;
       
        /// <summary>
        /// 
        /// Метод добавления сил кораблю для движения.
        /// </summary>
        private void UpdateRigidBody()
        {

            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.forward * Time.fixedDeltaTime, ForceMode.Force);

            m_Rigid.AddForce(-m_Rigid.linearVelocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode.Force);

            m_Rigid.AddTorque(-Vector3.up * (TorqueControl * m_Mobility * Time.fixedDeltaTime), ForceMode.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode.Force);
            
        }

        private void UpdateRigidBodyClimb()
        {

            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode.Force);

            m_Rigid.AddForce(-m_Rigid.linearVelocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode.Force);


        }


        /* #region Offensive
       
      [SerializeField] private Turret[] m_Turrets;
*/

        /// <summary>
        /// TODO: заменить временный метод-заглушку.
        /// Используется ИИ.
        /// </summary>
        /// <param name="mode"></param>
        public void Fire(TurretMode mode)
      {
          return;
      }
/*
      [SerializeField] private int m_MaxEnergy;
      [SerializeField] private int m_MaxAmmo;
      [SerializeField] private int m_EnergyRegenPerSecond;


      private float m_PrimaryEnergy;
      private int m_SecondaryAmmo;

      public int SecondaryAmmo => m_SecondaryAmmo;

      public void AddEnergy(int e)
      {
          m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);


      }

      public void AddAmmo(int ammo)
      {
          m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
      }

      private void InitOffensive()
      {
          m_PrimaryEnergy = m_MaxEnergy;
          m_SecondaryAmmo = m_MaxAmmo;
      }

      private void UpdateEnergyRegen()
      {
          m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
          m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
      }
        */

        /// <summary>
        /// TODO: Заменить временный метод-заглушку.
        /// Используется турелями.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawEnergy(int count)
      {
          return true;
      }

     /// <summary>
     /// TODO: Заменить временный метод-заглушку.
     /// Используется турелями.
     /// </summary>
     /// <param name="count"></param>
     /// <returns></returns>
      public bool DrawAmmo(int count)
      {
          return true;
      }
      /*
#endregion*/


        /*public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }*/



        public void AddMaxLinearVelocity(float bonusLinearVelocity)
        {
            m_MaxLinearVelocity += bonusLinearVelocity;
        }

        public void RemoveMaxLinearVelocity(float bonusLinearVelocity)
        {
            m_MaxLinearVelocity -= bonusLinearVelocity;
        }

        public void ResetSpeed()
        {
            m_Thrust = m_Stats.Thrust;
            m_Mobility = m_Stats.Mobility;
            m_MaxLinearVelocity = m_Stats.MaxLinearVelocity;
            m_MaxAngularVelocity = m_Stats.MaxAngularVelocity;
        }

        public void LoadRocket()
        {
           // AssignWeapon(m_Stats.SecondaryTurret);   
        }

        public void ResetRockets()
        {
          //  AssignWeapon(null);
        }

        public override void RecordMaxHP(int hitPoints)
        {
            if (Player.Instance.ActiveShip == this)
               Player.Instance.UsedData.ShipMaxHitPoints = hitPoints;
        }

       

     

        private void OnCollisionEnter2D(Collision2D collision)
        {
           // Debug.Log("Rerir " +  collision.gameObject.transform.root.name);
        }

        protected override void Use(TowerDefense.EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);
        }

        /// true air false ground
        private bool m_G;
        // Jumping is not Space ship but Beast movement method.
        public void Leap()
        {
            int count = 0;

            StartCoroutine(LeapTick());

            IEnumerator LeapTick()
            {
                m_Rigid.AddForce(m_Leap * (transform.up  + transform.forward * 0.5f), ForceMode.Force);
                count++;
                yield return new WaitForSeconds(0.05f);

                if (count < 5)
                    StartCoroutine(LeapTick());
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            m_G = false;
        }

        //private void OnCollisionExit(Collision collision)
        //{
        //    m_G = true;
        //}

        //gait
        public void SetMaxLinearVelocity(float maxLinearVelocity)
        {
            m_MaxLinearVelocity = maxLinearVelocity;
        }
    }
}

