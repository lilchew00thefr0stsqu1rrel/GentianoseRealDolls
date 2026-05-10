using System;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    // Original name: Stove
    public class InteractableObject : MonoBehaviour
    {
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
            print("Yoink");
            var partyWisp = other.GetComponent<Party>();
            if (partyWisp != null)
            {
                m_Dashboard.ShowInteractTip(tipID);
                OnDollCome(partyWisp);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var partyWisp = other.GetComponent<Party>();
            if (partyWisp != null)
            {
                m_Dashboard.HideInteractTip();
                OnDollGone(partyWisp);
            }
        }

        protected virtual void OnDollCome(Party p) { }
        protected virtual void OnDollGone(Party p) { }

    }
}

