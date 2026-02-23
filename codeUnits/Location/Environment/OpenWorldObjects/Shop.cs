using UnityEngine;

namespace GentianoseRealDolls
{
    public class Shop : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private int m_TipID = 7;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() != null)
            {
                Dashboard.Instance.ShowInteractTip(m_TipID);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() != null)
            {
                Dashboard.Instance.HideInteractTip();
            }
        }
    }
}

