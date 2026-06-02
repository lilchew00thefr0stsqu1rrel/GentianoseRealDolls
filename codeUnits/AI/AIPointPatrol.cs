using UnityEditor;
using UnityEngine;



namespace GentianoseRealDolls
{
    public class AIPointPatrol : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private float m_Height;
        public float Radius => m_Radius;
        public float Height => m_Height;

        private static readonly Color GizmoColor = new Color(0.8f, 0.7f, 1, 0.7f);

    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position + Vector3.up * m_Height * 0.5f, transform.up, m_Radius);
            Handles.DrawSolidDisc(transform.position + Vector3.up * m_Height * -0.5f, -transform.up, m_Radius);
        }
    #endif

    }
}

