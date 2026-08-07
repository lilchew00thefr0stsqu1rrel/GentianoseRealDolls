using UnityEngine;

namespace GentianoseRealDolls
{
    [CreateAssetMenu]
    public class DollAsset : ScriptableObject
    {
        public int DollID;
        public float AnalGlandVolume;
        public int MaxHitPoints;
        public Doll.Attribute Attribute;
        public Doll.ChemicalClass ChemicalClass;
        public int Defense;

        public Sprite RSkillIcon;
        public Sprite RSkillFill;
        public Sprite RChargeFill;  //~~~

        public MeshFilter PoopMesh;
        public Material PoopMaterial;
        public float PoopHeight;
        public float PoopRadius;

        public int PoopSize = 1;

        public Vector3 PoopScale;
        public float PoopMass;

        // ешка выдры заменяет её обычную атаку на царапание когтями, а заряженную на лягание
        public bool isLesserSkillAttackModifier;

        public Vector3 m_BedPos;

        [SerializeField] private int m_ModelSize;
        public int ModelSize => m_ModelSize;
    }

}

