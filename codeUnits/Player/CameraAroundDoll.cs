using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;


namespace GentianoseRealDolls
{

    public class CameraAroundDoll : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        // В универсальных квадроберских единицах (7 градуса 30 минут)
        [SerializeField] private int m_ThetaQuadrobist;
        [SerializeField] private int m_ThetaInput;


        // В вршк
        [Tooltip("В вршк")]
        [SerializeField] private int m_Radius = 22;
        [SerializeField] private float m_InterpolationLinear = 4;
        [SerializeField] private float m_InterpolationAngular = 5;

        [SerializeField] private float m_Height = 1;
        [SerializeField] private float m_BaseHeight = 1;
        [SerializeField] private float m_ExtraHeight = 2;
        [SerializeField] private float m_SmallBeastHeight = 0.5f;

        [SerializeField] private float m_MaxRadius = 0.3f;
        [SerializeField] private float m_MinRadius = 5f;
        [SerializeField] private float m_RadiusStep = 0.044f;


        [SerializeField] private float m_QuadrobistAngleStep = 7.5f;
        [SerializeField] private float m_InputAngleRatio = 6;
        [SerializeField] private int m_FullAngleInMarmosetUnits = 48;


        [SerializeField] private float m_TrueTargetDelta;
        private void Start()
        {
            m_Radius = 1;

            Normalize();
        }
        public void Normalize()
        {
            m_ThetaInput = 0;
        }
        // TODO: Lerp и Slerp
        private void FixedUpdate()
        {
            PoseCamera();
        }

        private void PoseCamera()
        {
            if (m_Target)
            {
                if (m_ThetaInput > m_FullAngleInMarmosetUnits * m_InputAngleRatio - 1) m_ThetaInput = 0;
                if (m_ThetaInput < 0) m_ThetaInput = (int)(m_FullAngleInMarmosetUnits * m_InputAngleRatio - 1);
                m_ThetaQuadrobist = (int)(m_ThetaInput / m_InputAngleRatio);


                if (m_Radius > 100) m_Radius = 100;
                if (m_Radius < 2) m_Radius = 2;

                Vector3 toDoll = m_Target.position + new Vector3(0, m_TrueTargetDelta, 0) - transform.position;

                float theta = m_ThetaQuadrobist * m_QuadrobistAngleStep * Mathf.Deg2Rad;

                transform.position = m_Target.TransformPoint(
                    new Vector3(Mathf.Sin(theta) * m_Radius * m_RadiusStep, m_Height,
                    -Mathf.Cos(theta) * m_Radius * m_RadiusStep));
                transform.position = new Vector3(transform.position.x, m_Target.position.y + m_Height, transform.position.z);

                Quaternion toDollLook = Quaternion.LookRotation(toDoll, Vector3.up);

                transform.rotation = toDollLook;

                transform.forward = toDoll;



            }

        }

        public void Zoom(int sign)
        {
            m_Radius += sign;
        }

        public void LookUp()
        {
            m_TrueTargetDelta = 1;
        }

        public void OffLookUp()
        {
            m_TrueTargetDelta = 0;
        }

        public void Rotate(int sign)
        {
            m_ThetaInput += sign;
        }

        public void BirdEye()
        {
            m_Height = m_ExtraHeight;
        }

        public void ReBirdEye(int dollSize)
        {
            if (dollSize == 0)
             m_Height = m_SmallBeastHeight;
            else
                m_Height = m_BaseHeight;
        }
        public void ReBirdEyeSmallDoll()
        {
            m_Height = m_SmallBeastHeight;
        }

        public void SetTarget(Transform transform)
        {
            m_Target = transform;
        }

        public void Turn()
        {
            m_ThetaQuadrobist += 48;
        }
    }
}
