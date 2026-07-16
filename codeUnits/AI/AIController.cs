using Common;
using System;
using UnityEngine;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol,
            Peaceful
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        [SerializeField] protected AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        [SerializeField] private float m_ReachPointRadius;

        [SerializeField] private float m_LeadSpeedMeasureTime;

        [SerializeField] private float m_LeadTime;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private GRDTimer m_RandomizeDirectionTimer;
        private GRDTimer m_FireTimer;
        private GRDTimer m_FindNewTargetTimer;


        private int m_PointIndex;


        private Stopwatch m_LeadMeasureStopwatch;
        private Stopwatch m_LeadUseStopwatch;

        private Rigidbody m_Rigid;


        private Rigidbody m_TargetRigid;  // ~


        // DeBuG
        [SerializeField] private float m_Azimuth;
        protected void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();

            m_Rigid = m_SpaceShip.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
            ActionChangeGait();
        }

        protected virtual void ActionChangeGait()
        {

        }

        private void ActionFindNewMovePosition()
        {
            //Debug.Log(m_MovePosition + " Dolls");

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)  // If there are some targets.
                {

                    m_MovePosition = MakeLead(m_TargetRigid);

                }
                else
                {
                    if (m_PatrolPoint  != null)
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).magnitude 
                            < m_PatrolPoint.Radius;


                        if (isInsidePatrolZone == true) // Near patrol point
                        {
                            GetNewPoint();

                            // print("Hyperborean snt snt");

                        }
                        else  // To patrol point
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                            
                        }
                    }


                }
            }

            if (m_AIBehaviour == AIBehaviour.Peaceful)
            {
                if (m_PatrolPoint != null)
                {
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).magnitude
                        < m_PatrolPoint.Radius;


                    if (isInsidePatrolZone == true) // Near patrol point
                    {
                        GetNewPoint();

                         print("Danica Click");
                    }

                    else  // To patrol point
                    {
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }
                }
            }
        }

        protected virtual void GetNewPoint()
        {
            if (m_RandomizeDirectionTimer.IsFinished == true)
            {
                m_MovePosition = m_PatrolPoint.transform.position + UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius;

                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);

                //Debug
                m_Azimuth = Vector3.SignedAngle(Vector3.forward, transform.InverseTransformPoint(m_PatrolPoint.transform.position), Vector3.up);
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.forward, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 1.0f;

                //Debug.Log("Imunlaukr");
            }
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;

        }

        private const float MAX_ANGLE = 45.0f;
        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector3 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            localTargetPosition.y = 0;
            float angle = Vector3.SignedAngle(Vector3.forward, localTargetPosition,  Vector3.up);


            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;



            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                if (m_SelectedTarget == null) m_TargetRigid = null;
                else m_TargetRigid = m_SelectedTarget.GetComponent<Rigidbody>();

                m_FindNewTargetTimer.Start(m_ShootDelay);
            }
        }
        private void ActionFire()
        {
            
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Direct);

                    m_FireTimer.Start(m_ShootDelay);
                }
            }
            
        }
        [SerializeField] private bool m_Friendly;

        private Destructible FindNearestDestructibleTarget()
        {
            if (m_SpaceShip.TeamId == 2) return null;
            
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            // in Gentianose Real Dolls
            if (m_Friendly) return null;

            foreach (var v in Destructible.AlLDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructible.TeamIdNeutral) continue;

                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = (Destructible)v;
                    // m_TargetRigid = v.GetComponent<Rigidbody2D>();
                }
            }

            return potentialTarget;
            
        }

        private Vector3 MakeLead(Rigidbody targetRigid)
        {
            if (targetRigid == null && m_PatrolPoint != null) return m_PatrolPoint.transform.position;

            if (targetRigid == null) return Vector3.zero;

            Vector3 targetVelocity = targetRigid.linearVelocity;


            // Distance between this ship (AI) and the target.
            float targetDistance = (targetRigid.transform.position - transform.position).magnitude;


            //Debug.Log("targetDistance: " + targetDistance);

            float shipFlightTime = targetDistance / m_Rigid.linearVelocity.magnitude;

            // Vector as line segment between current and lead positions.
            Vector3 leadLineSegmentVector = targetVelocity * shipFlightTime;

            Vector3 leadPoint = targetRigid.transform.position + leadLineSegmentVector;

            //Debug.Log(leadLineSegmentVector + " Kuutar");

            return leadPoint;
        }

        

        public void SetPatrolBehaviour(AIPointPatrol patrolPoint)
        {
            m_PatrolPoint = patrolPoint;
            m_AIBehaviour = AIBehaviour.Patrol;
        }

        public void SetProcessionBehaviour(AIPointPatrol patrolPoint)
        {
            m_PatrolPoint = patrolPoint;
            m_AIBehaviour = AIBehaviour.Patrol;
        }

        public void ResetPatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.Null;
            m_PatrolPoint = null;
        }
        protected void StopPatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.Null;
        }
        protected void StartPatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.Patrol;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new GRDTimer(m_RandomSelectMovePointTime);
            m_FireTimer = new GRDTimer(m_ShootDelay);
            m_FindNewTargetTimer = new GRDTimer(m_FindNewTargetTime);

           m_LeadMeasureStopwatch = new Stopwatch(m_LeadSpeedMeasureTime, "Whoo");
            m_LeadUseStopwatch = new Stopwatch(m_LeadTime, "Doll");
        }

        private void UpdateTimers()
        {
          //  print(m_RandomizeDirectionTimer != null);
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);

            m_LeadMeasureStopwatch.RemoveTime(Time.deltaTime);
            m_LeadUseStopwatch.RemoveTime(Time.deltaTime);
        }

        private void SetPatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.Patrol;
        }

        #endregion



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_MovePosition, 0.08f);

            //Gizmos.color = Color.yellow;
            //if (m_SelectedTarget != null)
            //Gizmos.DrawLine(m_SelectedTarget.transform.position, transform.position);
        }
#endif
    }
}

