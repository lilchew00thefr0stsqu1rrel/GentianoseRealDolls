using UnityEngine;

namespace GentianoseRealDolls
{
    public class SarvaToilet : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static bool CanPoop;

        [SerializeField] private Collider m_Collider;
        private void OnTriggerEnter(Collider other)
        {
            var dc = other.transform.root.GetComponent<DollController>();
            if (dc != null && dc.ActiveDollInPartyStatus)
                CanPoop = true;
        }

        private void OnTriggerExit(Collider other)
        {
            var dc = other.transform.root.GetComponent<DollController>();
            if (dc != null && dc.ActiveDollInPartyStatus)
                CanPoop = false;
        }
    }
}

