using UnityEngine;
using NTC.MonoCache;
using SpaceShooter;
using System.Collections;
using System.Collections.Generic;

namespace GentianoseRealDolls
{
    public class DollClimbing : DollComponent
    {
        [SerializeField] private Transform m_Nose;
        public Transform Nose => m_Nose;
        [SerializeField] private Transform m_Hand;
        [SerializeField] private SpaceShip m_PetAsSpaceShip;
        [SerializeField] private Rigidbody m_Rigid;
        [SerializeField] private float m_DistanceToWall = 0.14f;

        [SerializeField] private Transform m_FootRayOrigin;
        [SerializeField] private Transform m_HandRayOrigin;

        [SerializeField] private float m_InterpolationAngular = 2f;

        private bool m_IsClimbing = false;

        private void Awake()
        {

        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        private int climbProjectionYSign = 0;
        public int ClimbProjectionYSign => climbProjectionYSign;

        public void StartClimbing()
        {
           // m_PetAsSpaceShip.SetMoveMode(SpaceShip.MoveMode.Climb);
            climbProjectionYSign = 1;

            m_Animator.SetInteger("Autom", 17);
        }
        public void EndClimbing()
        {
           // m_PetAsSpaceShip.SetMoveMode(SpaceShip.MoveMode.Normal);
            climbProjectionYSign = 0;
            m_Animator.SetInteger("Autom", 2);
        }
        public void StartDescend()
        {
           // m_PetAsSpaceShip.SetMoveMode(SpaceShip.MoveMode.Descend);
            climbProjectionYSign = -1;
            m_Animator.SetInteger("Autom", 18);
        }
        [SerializeField] private float pitchUpdateTime = 3.45f;



        private void Update()
        {
            var rHit = Physics.RaycastAll(m_FootRayOrigin.position, -transform.parent.up, 0.05f);


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

            if (m_HandRayOrigin != null)
            {
                var fHit = Physics.RaycastAll(m_HandRayOrigin.position, -transform.parent.up, 0.05f);
                List<RaycastHit> foreHit = new List<RaycastHit>();
                for (int i = 0; i < fHit.Length; i++)
                {
                    if (fHit[i].collider.transform.root.GetComponent<Doll>() == null)
                    {
                        foreHit.Add(fHit[i]);
                    }
                }

                print($"Romokusy {gameObject.name} : {foreHit.Count}");

                if (rearHit.Count > 0 && foreHit.Count == 0)
                {
                    print("Duh");
                    // Поворот куклы на 60 о
                    var dollEuler = transform.parent.eulerAngles;
                    var dollRotation = transform.parent.rotation;
                    dollRotation = Quaternion.AngleAxis(90, transform.parent.right);
                    transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, dollRotation, m_InterpolationAngular * Time.deltaTime);

                    if (climbProjectionYSign == 0)
                        StartDescend();
                    else
                        EndClimbing();
                }


            }
            var nHit = Physics.RaycastAll(m_Nose.position, transform.parent.forward, 0.05f);

            List<RaycastHit> noseHit = new List<RaycastHit>();
            for (int i = 0; i < nHit.Length; i++)
            {
                if (nHit[i].collider.transform.root.GetComponent<Doll>() == null)
                {
                    noseHit.Add(nHit[i]);
                }
            }
            print($"Foushee {transform.name} + {noseHit.Count}");
            if (noseHit.Count > 0)
            {
                print("Douleur " + noseHit[0].collider.name);
                // Поворот куклы на 60 о
                var dollEuler = transform.parent.eulerAngles;
                var dollRotation = transform.parent.rotation;
                dollRotation = Quaternion.AngleAxis(-90, transform.parent.right);
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, dollRotation, m_InterpolationAngular * Time.deltaTime);


                StartClimbing();
            }
           


            if (Input.GetMouseButtonDown(1))
            {
                if (climbProjectionYSign == 0)
                    StartDescend();
                else
                    EndClimbing();
            }
            // Update is called once per frame

        }
    }
}

