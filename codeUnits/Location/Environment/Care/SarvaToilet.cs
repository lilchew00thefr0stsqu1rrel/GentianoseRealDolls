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

        [SerializeField] private Collider m_Collider;

        protected override void OnDollCome(Party p)
        {
            if (p != null)
                CanPoop = true;
        }

        protected override void OnDollGone(Party p)
        {
            if (p != null)
                CanPoop = false;
        }
    }
}

