using UnityEngine;

namespace GentianoseRealDolls
{
    // Original name: Stove
    public class InteractableObject : MonoBehaviour
    {
     
        [Tooltip("0 - stove; 1 - table; 6 - resource; 7 - shop")]
        [SerializeField] protected int tipID = 0;

      //  [SerializeField] private GameObject stoveUI;
        private string interactTip = "Приготовить";
        private void OnTriggerEnter(Collider other)
        {
            var doll = other.transform.root.GetComponent<DollController>();
            if (doll != null && doll.ActiveDollInPartyStatus)
            {
                OnDollCome(doll);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var doll = other.transform.root.GetComponent<DollController>();
            if (doll != null)
            {
                OnDollGone(doll);
            }
        }

        protected virtual void OnDollCome(DollController doll)
        {
            Dashboard.Instance.ShowInteractTip(tipID);
        }
        protected virtual void OnDollGone(DollController doll)
        {
            Dashboard.Instance.HideInteractTip();
        }
    }
}

