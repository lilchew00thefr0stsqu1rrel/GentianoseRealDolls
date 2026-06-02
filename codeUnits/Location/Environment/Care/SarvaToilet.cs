using UnityEngine;

namespace GentianoseRealDolls
{
    public class SarvaToilet : InteractableObject
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tipID = 10;
        }


        public static bool CanPoop;


        protected override void OnDollCome(Party p)
        {
            if (m_Dashboard)
                m_Dashboard.ShowInteractTip(tipID);
            if (p != null)
                CanPoop = true;
        }

        protected override void OnDollGone()
        {
            if (m_Dashboard)
                m_Dashboard.HideInteractTip();
                CanPoop = false;
        }
    }
}

