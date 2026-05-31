using UnityEngine;
namespace GentianoseRealDolls
{

    public class WhooSettings : MonoBehaviour
    {

        [SerializeField] private int m_NumberOfDolls = 3;

        public static int NumberOfDolls;

        void Awake()
        {
            NumberOfDolls = m_NumberOfDolls;
        }
        
        public const string fileNameTime = "timePrev.dat";
        public const string fileNameDoll = "doll.dat";
        public const string fileNamePos = "dPositions.dat";
        public const string fileNameSleep = "dInBeds.dat";
        public const string fileNameLoc = "scene_map.dat";
        public const string fileNameInv = "inventory.dat";
        public const string fileNamePoo = "pooStore.dat";

        public const string PathTime = "Assets/Resources/timePrev.dat";
        public const string PathDoll = "Assets/Resources/doll.dat";
        public const string PathPos = "Assets/Resources/dPositions.dat";
        public const string PathSleep = "Assets/Resources/dInBeds.dat";
        public const string PathLoc = "Assets/Resources/scene_map.dat";
        public const string PathInv = "Assets/Resources/inventory.dat";
        public const string PathPoo = "Assets/Resources/pooStore.dat";
    }

}
