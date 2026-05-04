using Common;
using UnityEngine;
using NTC.Pool;
using SpaceShooter;

namespace GentianoseRealDolls
{
    public class Turret : DollPart
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        [SerializeField] private ProjectileProperties m_ProjectileProperties;
        public void SetProjProps(ProjectileProperties projectileProperties)
        {
            m_ProjectileProperties = projectileProperties;
        }

        [SerializeField] private Camera m_Camera;

        private float m_ZOffsetAimSpray = 4;

        #region UnityEvent

        private void Start()
        {           
            //m_Ship = transform.parent.GetComponent<SpaceShip>();\
             
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
            else if (Mode == TurretMode.Auto)
            {
                Fire();                
            }
        }

        #endregion

        #region Public API

        private enum TurretTrajectory
        {
            None,
            Thorn,
            Direct
        }

        [SerializeField] private TurretTrajectory m_Trajectory; 

        // стар. 
        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship)
            {
                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                    return;

                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                    return;
            }

            if (m_Trajectory == TurretTrajectory.Thorn)
            {
                var aim = m_Camera.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, Input.mousePosition.y, m_ZOffsetAimSpray),
                Camera.MonoOrStereoscopicEye.Mono);
                var aimedVector = aim - transform.position;

                // Направление фуньки
                print(Vector3.Angle(aimedVector, transform.forward));

                transform.forward = aimedVector;
            }
           


            if (m_Trajectory == TurretTrajectory.Direct)
            {


                var aimedVector = m_Ship.transform.forward;
                aimedVector.y = 0.3f;
                transform.forward = aimedVector;
            }


            Projectile projectile = NightPool.Spawn(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.SetProperties(m_ProjectileProperties);
            projectile.transform.position = transform.position;
            projectile.transform.forward = transform.forward;



            projectile.SetParentShooter(m_Ship);
            projectile.SetAoEParent(m_Ship);
            m_RefireTimer = m_TurretProperties.RateOfFire;

              

            {
                // SFX
            }
        }

        // Нов.
        public void Fire(Vector2 aimInput)
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship)
            {
                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                    return;

                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                    return;
            }

            if (m_Trajectory == TurretTrajectory.Thorn)
            {
                var aim = m_Camera.ScreenToWorldPoint(new Vector3(
                aimInput.x, aimInput.y, m_ZOffsetAimSpray),
                Camera.MonoOrStereoscopicEye.Mono);
                var aimedVector = aim - transform.position;

                // Направление фуньки
                print(Vector3.Angle(aimedVector, transform.forward));

                transform.forward = aimedVector;
            }



            if (m_Trajectory == TurretTrajectory.Direct)
            {


                var aimedVector = m_Ship.transform.forward;
                aimedVector.y = 0.3f;
                transform.forward = aimedVector;
            }


            Projectile projectile = NightPool.Spawn(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.SetProperties(m_ProjectileProperties);
            projectile.transform.position = transform.position;
            projectile.transform.forward = transform.forward;



            projectile.SetParentShooter(m_Ship);
            projectile.SetAoEParent(m_Ship);
            m_RefireTimer = m_TurretProperties.RateOfFire;



            {
                // SFX
            }
        }

        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode) return;

            m_RefireTimer = 0;

            m_TurretProperties = props;
        }


        public void SetCamera(Camera cam)
        {
            m_Camera = cam;
        }
        #endregion
    }

}
