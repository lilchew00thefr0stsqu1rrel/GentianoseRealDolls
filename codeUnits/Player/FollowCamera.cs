using GentianoseRealDolls;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using VContainer;
namespace SpaceShooter
{
    public class FollowCamera : MonoBehaviour, ICamera
    {
        [SerializeField] private Transform m_Target;

        [SerializeField] private float m_InterpolationLinear;

        [SerializeField] private float m_InterpolationAngular;

        [SerializeField] private float m_CameraBackOffset;

        [SerializeField] private float m_ForwardOffset;

        [SerializeField] private float m_CameraBackStep = 0.1f;
        [SerializeField] private float m_CameraBackStepBirdEye = 0.016f;
        [SerializeField] private float m_CameraUpStep = 0.05f;
        [SerializeField] private float m_CameraUpStepBirdEye = 0.1f;

        [SerializeField] private float m_UpOffset; // back up like a cat urine spray
        [SerializeField] private float m_UpAngleOffset;

        [SerializeField] private bool m_DoNotDestroyOnLoad;
        [SerializeField] private Camera m_ProperCamera;
        public Camera ProperCamera => m_ProperCamera;
        

        // 1, ���� ������ ������� ����� ������������ �����; -1, ���� �����
        private int m_Forward = 1;

        [SerializeField] private int m_MaxCameraBackOffset;

        [SerializeField] private float m_MinCameraBackOffset;
        [SerializeField] private float m_MaxUpOffset;

        [SerializeField] private float m_MinUpOffset;

        private void Awake()
        {
            if (m_DoNotDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        [SerializeField] private float zz;


        float mouseScrollInput;
        // ���� ����� ���������� Player Input,
        // ����� ����������� �������� Move
        public void OnZoom(InputAction.CallbackContext context)
        {
            // ��������� �������� Vector2 �� Input System
            // ��� ��������� ����������� ��������
            mouseScrollInput = context.ReadValue<float>();
        }

        private bool m_BirdEye;
        private void FixedUpdate()
        {
            if (m_Target == null) return;




            //  if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
            if (mouseScrollInput > 0.05f)
            {
                if (!m_BirdEye && m_CameraBackOffset < m_MaxCameraBackOffset)
                {
                    m_CameraBackOffset += m_CameraBackStep;
                    m_UpOffset += m_CameraUpStep;
                }
                if (m_BirdEye && m_UpOffset < m_MaxUpOffset)
                {
                    m_UpOffset += m_CameraUpStepBirdEye;
                    m_CameraBackOffset += m_CameraBackStepBirdEye;
                }
            }
            //if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
            if (mouseScrollInput < -0.05f)
                {
                if (!m_BirdEye && m_CameraBackOffset > m_MinCameraBackOffset)
                {
                    m_CameraBackOffset -= m_CameraBackStep;
                    m_UpOffset -= m_CameraUpStep;
                }
                if (m_BirdEye && m_UpOffset > m_MinUpOffset)
                {
                    m_UpOffset -= m_CameraUpStepBirdEye;
                    m_CameraBackOffset -= m_CameraBackStepBirdEye;
                }
            }



            Vector3 camPos = transform.position;


            Vector3 targetPos = m_Target.position + m_CameraBackOffset * -m_Target.transform.forward;

            targetPos.y = m_Target.position.y + m_UpOffset;


           
            Vector3 newCamPos = Vector3.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);


            Vector3 targetBack = -m_Target.transform.forward;
            transform.position = new Vector3(newCamPos.x, newCamPos.y, newCamPos.z);



            Vector3 petPos = m_Target.position;
            petPos += m_Target.transform.forward * 0.62f;
            Vector3 downToDoll = petPos - camPos;
            downToDoll.y = -m_UpOffset;


           
            //print("Targ " + targetPos);
            Vector3 antiDownToDoll = new Vector3(-downToDoll.x, downToDoll.y, -downToDoll.z);


            Quaternion directionQuat = 
                Quaternion.LookRotation(downToDoll, Vector3.up);

            

            if (m_InterpolationAngular > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                              directionQuat,
                                                              m_InterpolationAngular * Time.deltaTime);

            }



            
        }

        [SerializeField] private float m_CameraBackNormal = 0.3f;
        [SerializeField] private float m_CameraBackBirdEye = -0.07f;
        [SerializeField] private float m_CameraBackWhenReverse = -3;



        [SerializeField] private float m_CameraBackWisp = 1;
        [SerializeField] private float m_CameraUpLookWisp = 0.2f;

        [SerializeField] private float m_UpBirdEye = 1.7f;
        [SerializeField] private float m_UpNormal = 0.4f;



        public void SetTarget(Transform newTarget)
        {
            m_Target = newTarget;
        }
        public void SetMinOffsetWisp()
        {
            m_MinCameraBackOffset = m_CameraBackWisp;
            m_CameraBackOffset = m_CameraBackWisp;
            m_MinUpOffset = m_CameraUpLookWisp;
            m_UpOffset = m_CameraUpLookWisp;
        }
        public void SetMinOffsetDoll()
        {
            m_MinCameraBackOffset = m_CameraBackNormal;
            m_CameraBackOffset = m_CameraBackNormal;
            m_MinUpOffset = m_UpNormal;
            m_UpOffset = m_UpNormal;
        }

      


        public void Turn(int forward)
        {
            print("Malipo");
            m_Forward = forward;

            m_CameraBackOffset = (forward == 1) ? m_CameraBackNormal : m_CameraBackWhenReverse;

  //          m_CameraBackOffset += 10f * forward;
        }
        public void FreezeRotation()
        {
            print("Malipo");
            m_InterpolationAngular = 0;
        }
        public void BirdEye()
        {
            m_BirdEye = true;
            print("Malipo");
            m_CameraBackOffset = m_CameraBackBirdEye;
            m_UpOffset = m_UpBirdEye;
        }
        public void ReBirdEye()
        {
            m_BirdEye = false;
            print("Malipo");
            m_CameraBackOffset = m_CameraBackNormal;
            m_UpOffset = m_UpNormal;
        }


    }
}

