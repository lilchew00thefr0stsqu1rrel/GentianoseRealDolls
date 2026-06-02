using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public class Shop : InteractableObject
    {
        /// <summary>
        /// Русикова, Пунова...
        /// BDUF - антипаттерн
        /// /// </summary>

        [SerializeField] private int m_ShopID;
        private void Start()
        {
            tipID = 7;
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.transform.root.GetComponent<Doll>() != null)
        //    {
        //        m_Dashboard.ShowInteractTip(tipID);
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.transform.root.GetComponent<Doll>() != null)
        //    {
        //        m_Dashboard.HideInteractTip();
        //    }
        //}
    }
}

