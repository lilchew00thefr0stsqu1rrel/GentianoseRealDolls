using NTC.MonoCache;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class EnterHabitat : MonoCache
    {
        [SerializeField] private Door door;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
          
        }
        bool Near;


        private int tipID = 3;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() != null)
            {
                Dashboard.Instance.ShowInteractTip(tipID);

                Near = true;
            }
        }

        protected override void Run()
        {
            base.Run();

            if (Near)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    door.Activate();
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.GetComponent<Doll>() != null)
            {
                Dashboard.Instance.HideInteractTip();


                Near = false;
            }
        }


        [SerializeField] private Canvas Interact;
    }
}

