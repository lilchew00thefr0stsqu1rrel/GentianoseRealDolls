using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Common
{
    public class CylinderArea : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        [SerializeField] private float m_Height;


        [SerializeField] private Transform m_Cylinder;

        private void Start()
        {
            if (m_Cylinder == null) return;

            m_Cylinder.localScale = new Vector3(m_Radius * 2, m_Height * 0.5f, m_Radius * 2);
        }

        public Vector3 GetRandomInsideZone()
        {
            Vector2 rndCircle = Random.insideUnitCircle;
            rndCircle *= m_Radius;
            Vector3 rndCylinder = new Vector3(rndCircle.x, Random.Range(-0.5f, 0.5f) * m_Height, rndCircle.y);

            return transform.position + rndCylinder;
        }

#if UNITY_EDITOR
        //private static Color GizmoColor = new Color(0, 1, 0, 0.3f);

        //private void OnDrawGizmosSelected()
        //{
        //    Handles.color = GizmoColor;
        //    Handles.DrawSolidDisc(transform.position + Vector3.up * m_Height * 0.5f, Vector3.up, m_Radius);
        //    Handles.DrawSolidDisc(transform.position + Vector3.up * m_Height * -0.5f, -Vector3.up, m_Radius);
        //}
#endif

    }

}
