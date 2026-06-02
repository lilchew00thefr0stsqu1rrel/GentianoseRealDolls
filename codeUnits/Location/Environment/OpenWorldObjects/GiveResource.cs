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
        public static event Action OnLeaveResource;

        public static event Action<InventoryItem, int> OnTake;

        [SerializeField] private InventoryItem m_Item;
        [SerializeField] private int m_YieldAmount;

        [SerializeField] private List<GameObject> m_ItemsInWorld;

        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }
        [SerializeField] private Inventory m_Inventory;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            print("00");
            __ = false; 
            tipID = 6;
        }

        private DateTime useDateTime;

        private bool __;


        protected override void OnDollCome(Party p)
        {
            if (m_Item != null && !__ && m_ItemsInWorld.Count > 0)
            {
                print("Resources!");
                OnWentToResource(tipID, m_Item.itemName, this);
            }
        }

        protected override void OnDollGone()
        {
            OnLeaveResource();
            print("Awayy~~!");
        }
      

        public void GiveResources()
        {
            if (m_ItemsInWorld.Count > 0)
            {
                OnTake(m_Item, 1);
                //m_Inventory.AddItemInstances(m_Item, 1);
                print("do");
                InventoryController.Instance.InitAllItems();
                NightPool.Despawn(m_ItemsInWorld[0]);
                m_ItemsInWorld.RemoveAt(0);


                if (m_ItemsInWorld.Count == 0)
                {
                    OnLeaveResource();
                }
            }
            
        }

    }

}
