using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GentianoseRealDolls
{
    public class ZifferGate : MonoBehaviour
    {
        [SerializeField] private int m_LevelID;

        [SerializeField] private TeleportBeasts m_Teleporter;

        [Inject]
        public void Construct(TeleportBeasts teleportBeasts)
        {
            m_Teleporter = teleportBeasts;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //var tb = FindAnyObjectByType<TeleportBeasts>();
           
            m_Teleporter.InitScene(m_LevelID);

        }
    }
}

