using System;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    // Original name: Stove
    public class InteractableObject : MonoBehaviour
    {

        public static event Action<int> OnCameToInteract;

        protected Dashboard m_Dashboard;

        [Inject]
        public void Construct(Dashboard obj)
        {
            m_Dashboard = obj;
        }

        [Tooltip("0 - stove; 1 - table; 6 - resource; 7 - shop")]
        [SerializeField] protected int tipID = 0;


        private void OnTriggerEnter(Collider other)
        {
            var partyWisp = other.GetComponent<Party>();
            if (partyWisp != null)
            {
                print("Yoink" + gameObject.name + other.name);
                OnDollCome(partyWisp);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var partyWisp = other.GetComponent<Party>();
            if (partyWisp != null)
            {
                OnDollGone(partyWisp);
            }
        }
        
        

        protected virtual void OnDollCome(Party p)
        {
            if (m_Dashboard)
                m_Dashboard.ShowInteractTip(tipID);
        }
        protected virtual void OnDollGone(Party p)
        {
            if (m_Dashboard)
                m_Dashboard.HideInteractTip();
        }

    }
}

