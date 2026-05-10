using UnityEngine;
using NTC.MonoCache;
using SpaceShooter;
using System.Collections;

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
            m_PetAsSpaceShip.SetMoveMode(SpaceShip.MoveMode.Climb);
            climbProjectionYSign = 1;
        }
        public void EndClimbing()
        {
            m_PetAsSpaceShip.SetMoveMode(SpaceShip.MoveMode.Normal);
            climbProjectionYSign = 0;
        }
        public void StartDescend()
        {
            m_PetAsSpaceShip.SetMoveMode(SpaceShip.MoveMode.Descend);
            climbProjectionYSign = -1;
        }
        [SerializeField] private float pitchUpdateTime = 3.45f;



        [SerializeField] private float m_InterpolationAngular;
        private void Update()
        {
            var hit = Physics.RaycastAll(transform.parent.position, -transform.parent.up, 0.2f);

            if (hit != null)
            {
                if (hit.Length > 0)
                {
                    
                    if (hit[0].collider.transform.root.GetComponent<Doll>() == null 
                        && Mathf.Min(Vector3.Angle(Vector3.up, hit[0].normal), 180 - Vector3.Angle(Vector3.up, hit[0].normal)) > 15)
                    {
                        var upNormal = hit[0].normal;
                        var dollRotation = transform.parent.rotation;
                        dollRotation = Quaternion.Euler(upNormal.x, dollRotation.y, dollRotation.z);

                    }
                }
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

