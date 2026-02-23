using UnityEngine;

namespace GentianoseRealDolls
{
    public class EnterHabitat : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
          
        }


        private int tipID = 3;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() != null)
            {
                Dashboard.Instance.ShowInteractTip(tipID);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() != null)
            {
                Dashboard.Instance.HideInteractTip();
            }
        }


        [SerializeField] private Canvas Interact;
    }
}

