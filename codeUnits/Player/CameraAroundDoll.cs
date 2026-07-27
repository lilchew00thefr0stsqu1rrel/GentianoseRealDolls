using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using Unity.VisualScripting;


namespace GentianoseRealDolls
{

    public class CameraAroundDoll : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        [SerializeField] private float m_InterpolationLinear = 4;
        [SerializeField] private float m_InterpolationAngular = 5;

        // В универсальных квадроберских единицах (7 градуса 30 минут)
        [SerializeField] private int m_ThetaRaw;
        [SerializeField] private int m_Theta;

        [SerializeField] private float m_QuadrobistAngleStep = 7.5f;
        [SerializeField] private float m_InputAngleRatio = 6;
        [SerializeField] private int m_FullAngleInMarmosetUnits = 48;

        [SerializeField] private float m_LinearStep = 0.1f;
        
        [SerializeField] private float m_Radius = 1;
        [SerializeField] private float m_MinRadius = 0.3f;
        [SerializeField] private float m_MaxRadius = 5f;

        [SerializeField] private float m_Height = 1;

        [SerializeField] private float m_BaseHeight = 1;
        [SerializeField] private float m_ExtraHeight = 2;
        [SerializeField] private float m_SmallBeastHeight = 0.5f;

        [SerializeField] private bool m_AimMode;

        [SerializeField] private int m_TargetUpOffset;

        private void Start()
        { 

            Normalize();
        }
        public void Normalize()
        {
            m_ThetaRaw = 0;
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
                if (m_ThetaRaw >= 360) m_ThetaRaw = 0;
                if (m_ThetaRaw < 0) m_ThetaRaw = 359;

                m_Theta = (int)(m_ThetaRaw / m_InputAngleRatio);

                m_Radius = Mathf.Clamp(m_Radius, m_MinRadius, m_MaxRadius);


                Vector3 toDoll = m_Target.position - transform.position;



                float theta = m_Theta * m_QuadrobistAngleStep * Mathf.Deg2Rad;

                transform.position = m_Target.TransformPoint(
                    new Vector3(Mathf.Sin(theta) * (int)m_Radius, (int)m_Height,
                    -Mathf.Cos(theta) * (int)m_Radius));

                Quaternion toDollLook = Quaternion.LookRotation(toDoll, Vector3.up);

                transform.rotation = toDollLook;

                transform.forward = toDoll;
            }
        }


        public void AimMode()
        {
            m_Height = 0;
            m_Radius = 1;

            m_AimMode = true;
        }

        public void OffAimMode(int dollSize)
        {
            if (dollSize == 0)
                m_Height = m_SmallBeastHeight;
            else
                m_Height = m_BaseHeight;

            m_Radius = 1;

            m_AimMode = false;
        }
        public void Zoom(int sign)
        {
            m_Radius += sign * 0.1f;
        }

        public void Rotate(int sign)
        {
            m_ThetaRaw += sign;
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

            m_Radius = 1;
        }

        public void SetTarget(Transform transform)
        {
            m_Target = transform;
        }

        public void Turn()
        {
            m_Theta += 48;
        }

    }
}
