using UnityEngine;
using SpaceShooter;
using Common;

namespace TowerDefense
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CylinderArea startArea;
        public CylinderArea StartArea { get { return startArea; } } 
        [SerializeField] private AIPointPatrol[] points;
        public int Length { get => points.Length; } 
        public AIPointPatrol this[int i] { get => points[i]; }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point.transform.position, point.Radius);
            }
            
        }
#endif
    }
}

