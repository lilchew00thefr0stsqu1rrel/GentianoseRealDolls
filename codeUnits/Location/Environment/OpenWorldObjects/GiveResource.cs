using NTC.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GentianoseRealDolls
{
    public class GiveResource : InteractableObject
    {
        public static event Action<int, string, GiveResource> OnWentToResource;

        [SerializeField] private InventoryItem m_Item;
        [SerializeField] private int m_YieldAmount;

        [SerializeField] private List<GameObject> m_ItemsInWorld;

        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            print("00");
            __ = false; 
            tipID = 6;
        }

        private DateTime useDateTime;

        private bool __;
        

        private void OnTriggerEnter(Collider other)
        {
            if (m_Item != null && !__ && m_ItemsInWorld.Count > 0)
            {
                var p = other.GetComponent<Party>();
                if (p != null)
                {
                    print("Boing");


                    // m_Dashboard.ShowInteractTip(tipID, m_Item.itemName, this);
                    OnWentToResource(tipID, m_Item.itemName, this);

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
                NightPool.Despawn(m_ItemsInWorld[0]);
                m_ItemsInWorld.RemoveAt(0);
            }
            
        }

    }

}
