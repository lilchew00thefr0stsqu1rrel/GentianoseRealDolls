using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(BoxCollider))]
    // Original name: Stove
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private BoxCollider m_Collider;

        public static event Action<int> OnCameToInteract;

        protected Dashboard m_Dashboard;

        [Inject]
        public void Construct(Dashboard obj)
        {
            m_Dashboard = obj;
        }
        private void Awake()
        {
            m_Collider = GetComponent<BoxCollider>();
        }


        [Tooltip("0 - stove; 1 - table; 6 - resource; 7 - shop")]
        [SerializeField] protected int tipID = 0;
        [SerializeField] Party partyWisp;




        private void OnTriggerEnter(Collider other)
        {
            partyWisp = other.GetComponent<Party>();
            if (partyWisp != null)
            {
                print("Yoink" + gameObject.name + other.name);
                OnDollCome(partyWisp);
            }
        }
        float time = 0;

        private void OnTriggerStay(Collider other)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                partyWisp = other.GetComponent<Party>();
                if (partyWisp != null)
                {
                    print("Yoink" + gameObject.name + other.name);
                    OnDollCome(partyWisp);
                }

                time = 0;
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            partyWisp = other.GetComponent<Party>();
            if (partyWisp != null)
            {
                OnDollGone();
                partyWisp = null;
            }
        }
        
        

        protected virtual void OnDollCome(Party p)
        {
            if (m_Dashboard)
                m_Dashboard.ShowInteractTip(tipID);
        }
        protected virtual void OnDollGone()
        {
            if (m_Dashboard)
                m_Dashboard.HideInteractTip();
        }

    }
}

