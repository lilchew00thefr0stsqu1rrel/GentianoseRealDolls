using System;
using UnityEngine;
using System.Collections.Generic;

namespace GentianoseRealDolls
{
    public class GiveResource : MonoBehaviour
    {
        [SerializeField] private InventoryItem m_Item;
        [SerializeField] private int m_YieldAmount;

        [SerializeField] private List<GameObject> m_ItemsInWorld;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            print("00");
            __ = false;
        }

        private DateTime useDateTime;
        // Update is called once per frame
        void Update()
        {
          
        }
        private bool __;
        int tipID = 6;
        private void OnTriggerEnter(Collider other)
        {
            if (m_Item != null && !__ && m_ItemsInWorld.Count > 0)
            {
                if (other.transform.root.GetComponent<Doll>() != null)
                {
                    Dashboard.Instance.ShowInteractTip(tipID, m_Item.itemName, this);
                }
            }

            print(Inventory.Instance != null);
        }
        public void GiveResources()
        {
            if (m_ItemsInWorld.Count > 0)
            {
                Inventory.Instance.AddItemInstances(m_Item, 1);
                print("do");
                InventoryController.Instance.InitAllItems();
                Destroy(m_ItemsInWorld[0]);
                m_ItemsInWorld.RemoveAt(0);
            }
            
        }

    }

}
