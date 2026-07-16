using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public class SwimZone : MonoBehaviour
    {
        [SerializeField] private Party m_Party;

        [Inject]
        public void Construct(Party obj)
        {
            m_Party = obj;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() == null) return;

            m_Party.SetSwimming(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() == null) return;

            m_Party.SetSwimming(false);
        }
    }
}
