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

        public GameObject Model;


        public MeshFilter PoopMesh;
        public Material PoopMaterial;
        public float PoopHeight;
        public float PoopRadius;

        public int PoopSize = 1;

        public Vector3 PoopScale;
        public float PoopMass;

        // ешка выдры замен€ет еЄ обычную атаку на царапание когт€ми, а зар€женную на л€гание
        public bool isLesserSkillAttackModifier;
    }

}

