using UnityEngine;

namespace Common
{
    [CreateAssetMenu]
    public class ProjectileProperties : ScriptableObject
    {
        [SerializeField] private float m_Velocity;

        public float Velocity => m_Velocity;

        [SerializeField] private float m_Lifetime;

        public float Lifetime => m_Lifetime;

        public int Damage;

        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        public ImpactEffect ImpactEffectPrefab => m_ImpactEffectPrefab;

        [SerializeField] private Sprite m_VisualSprite;
        public Sprite VisualSprite => m_VisualSprite;

        [SerializeField] private float m_ColliderRadius;
        public float ColliderRadius => m_ColliderRadius;

        [SerializeField] private int m_HitAmount;

        public int HitAmount => m_HitAmount;
    }

}
