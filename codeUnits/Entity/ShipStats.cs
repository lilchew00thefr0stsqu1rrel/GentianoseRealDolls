using UnityEngine;

namespace GentianoseRealDolls
{
    [CreateAssetMenu]
    public class ShipStats : ScriptableObject
    {
        /// <summary>
        /// Толкающая вперёд сила
        /// </summary>
        [SerializeField] private float m_Thrust;
        public float Thrust => m_Thrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;
        public float Mobility => m_Mobility;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float MaxLinearVelocity => m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная вращательная скорость. В градусах.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        [SerializeField] private TurretProperties m_SecondaryTurret;
        public TurretProperties SecondaryTurret => m_SecondaryTurret;

        
    }
}

