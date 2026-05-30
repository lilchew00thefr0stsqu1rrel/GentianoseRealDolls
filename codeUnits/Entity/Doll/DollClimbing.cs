using UnityEngine;
using NTC.MonoCache;
using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace GentianoseRealDolls
{
    public class DollClimbing : MonoCache
    {
        [SerializeField] private BoxCollider m_NoseButton;
        [SerializeField] private BoxCollider m_HandButton;

        [SerializeField] private Transform m_Hand;
        [SerializeField] private SpaceShip m_PetAsSpaceShip;
        [SerializeField] private Rigidbody m_Rigid;
        [SerializeField] private Animator m_Animator;

        [SerializeField] private float m_DistanceToWall = 0.14f;

        [SerializeField] private Transform m_FootRayOrigin;
        [SerializeField] private Transform m_HandRayOrigin;

        [SerializeField] private float m_InterpolationAngular = 2f;

        [SerializeField] private float m_DistanceToWallNose = 0.07f;
        [SerializeField] private float m_DistanceToFloorFoot= 0.14f;
        [SerializeField] private float m_DistanceToFloorHand = 0.2f;

        // TODO: 60
        [SerializeField] private float m_ClimbThreshold = 30;
        [SerializeField] private float m_ClimbNormalForce = 500;

        private float m_HillAngle;
        public float HillAngle => m_HillAngle;

        private Vector2 moveInput;
        public void SetInput(Vector2 input)
        {
            moveInput = input;
        }
        public enum ClimbMode
        {
            NonClimb,
            Ascend,
            Descend
        }
  

        private ClimbMode m_DollClimbMode;
        public ClimbMode DollClimbMode => m_DollClimbMode;
        public void StartClimbing()
        {
            print("Rondo!!");
            if (m_DollClimbMode == ClimbMode.NonClimb)
            {
                m_DollClimbMode = ClimbMode.Ascend;

                m_Animator.SetInteger("Autom", 17);
            }
        }
        public void EndClimbing(int g)
        {
            print("Rondo!");
            if (m_DollClimbMode != ClimbMode.NonClimb)
            {
                m_DollClimbMode = ClimbMode.NonClimb;

                m_Animator.SetInteger("Autom", g);
            }
        }

        public void StartDescend()
        {
            print("Rondo!!!");
            if (m_DollClimbMode == ClimbMode.NonClimb)
            {
                m_DollClimbMode = ClimbMode.Descend;

                m_Animator.SetInteger("Autom", 18);
            }

        }

        public void ChangeClimb(int gait)
        {
            var front = Physics.OverlapBox(m_NoseButton.center, m_NoseButton.bounds.extents);
            if (front != null && front.Length != 0)
            {
                StartClimbing();
            }
            else
            {
                EndClimbing(gait);
            }

            var hands = Physics.OverlapBox(m_HandButton.center, m_HandButton.bounds.extents);
            if (hands != null && hands.Length == 0)
            {
                StartDescend();
            }

            else
            {
                EndClimbing(gait);
            }
        }



        protected override void Run()
        {
            m_HillAngle = Vector3.SignedAngle(Vector3.up, transform.parent.up, transform.parent.right);

         
                
            if (m_DollClimbMode != ClimbMode.NonClimb)
            {
                m_Rigid.AddForce(-transform.parent.up * m_ClimbNormalForce);
            }

            var rHit = Physics.RaycastAll(m_FootRayOrigin.position, -transform.parent.up, m_DistanceToFloorFoot);


            List<RaycastHit> rearHit = new List<RaycastHit>();
            for (int i = 0; i < rHit.Length; i++)
            {
                if (rHit[i].collider.transform.root.GetComponent<Doll>() == null)
                {
                    rearHit.Add(rHit[i]);
                }
            }


            if (rearHit != null)
            {
                if (rearHit.Count > 0)
                {
                    
                    if (Mathf.Min(Vector3.Angle(Vector3.up, rearHit[0].normal), 180 - Vector3.Angle(Vector3.up, rearHit[0].normal)) > 15)
                    {
                        var upNormal = rearHit[0].normal;
                        
                        var dollEuler = transform.parent.eulerAngles;
                        var dollRotation = transform.parent.rotation;
                        
                        dollRotation = Quaternion.Euler(Vector3.Angle(Vector3.up, rearHit[0].normal), dollEuler.y, dollEuler.z);

                        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, dollRotation, m_InterpolationAngular * Time.deltaTime);
                    }
                }
            }
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(m_FootRayOrigin.position, m_FootRayOrigin.position - transform.parent.up * m_DistanceToFloorFoot);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_HandRayOrigin.position, m_HandRayOrigin.position - transform.parent.up * m_DistanceToFloorHand); 

        }
    }
}


