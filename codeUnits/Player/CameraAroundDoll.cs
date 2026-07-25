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

        // Схема: величина float для ввода, int для фиксации и управления камерой
        // Шаг: 1 дм
        //

        [SerializeField] private float m_LinearStep = 0.1f;
        //
        [SerializeField] private float m_RadiusRaw = 1;
        [SerializeField] private int m_Radius;
        [SerializeField] private float m_MaxRadius = 5f;
        [SerializeField] private float m_MinRadius = 0.3f;

        [Tooltip("В вршк")]

        // В 10-см единицах
        [SerializeField] private float m_HeightRaw = 1;
        [SerializeField] private int m_Height = 10;

        [SerializeField] private float m_BaseHeight = 1;
        [SerializeField] private float m_ExtraHeight = 2;
        [SerializeField] private float m_SmallBeastHeight = 0.5f;

        [SerializeField] private bool m_AimMode;





        [SerializeField] private float m_TargetUpOffsetRaw;
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

                if (m_RadiusRaw > 5) m_RadiusRaw = 0.1f;
                if (m_RadiusRaw < 0.1f) m_RadiusRaw = 5;

                m_Radius = (int)(m_RadiusRaw / m_LinearStep);
                m_TargetUpOffset = (int)(m_TargetUpOffsetRaw / m_LinearStep);

                var dmx = (float)Math.Round(m_Target.position.x, 1);
                var dmy = (float)Math.Round(m_Target.position.y, 1);
                var dmz = (float)Math.Round(m_Target.position.z, 1);
                var dpos = new Vector3(dmx, dmy, dmz);
                Vector3 toDoll = dpos + Vector3.up * m_TargetUpOffsetRaw - transform.position;



                float theta = m_Theta * m_QuadrobistAngleStep * Mathf.Deg2Rad;

                transform.position = m_Target.TransformPoint(
                    new Vector3(Mathf.Sin(theta) * m_Radius * m_LinearStep, m_Height,
                    -Mathf.Cos(theta) * m_Radius * m_LinearStep));
                transform.position = new Vector3(transform.position.x, m_Target.position.y + m_Height * 0.1f, transform.position.z);

                Quaternion toDollLook = Quaternion.LookRotation(toDoll, Vector3.up);

                transform.rotation = toDollLook;

                transform.forward = toDoll;



            }

        }


        public void LookUp()
        {
        }

        public void OffLookUp()
        {
        }

        public void AimMode()
        {
            m_TargetUpOffsetRaw = 1.1f;

            m_AimMode = true;
        }

        public void OffAimMode()
        {

            m_TargetUpOffsetRaw = 0.6f; 


            m_AimMode = false;
        }
        public void Zoom(int sign)
        {
            m_RadiusRaw += sign * 0.1f;
        }

        public void Rotate(int sign)
        {
            m_ThetaRaw += sign;
        }

        public void Lift(int sign)
        {
            m_HeightRaw += sign * 0.1f; ;
        }

        public void BirdEye()
        {
            m_HeightRaw = m_ExtraHeight;
        }

        public void ReBirdEye(int dollSize)
        {
            if (dollSize == 0)
             m_HeightRaw = m_SmallBeastHeight;
            else
                m_HeightRaw = m_BaseHeight;
        }
        public void ReBirdEyeSmallDoll()
        {
            m_HeightRaw = m_SmallBeastHeight;
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
