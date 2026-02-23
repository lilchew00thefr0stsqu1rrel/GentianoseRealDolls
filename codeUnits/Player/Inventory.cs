using System;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense;
using System.Linq;
using Unity.VisualScripting;


namespace GentianoseRealDolls
{
    [Serializable]
    public class ItemAmount
    {
        //public InventoryItem item;
        public int amount;
        //

        public int itemID;
        public string itemName;
        public ItemCategory category;

        //public void AddItem(List<InventoryItem> itemsList, List<int> amountList)
        //{
        //    itemsList.Add(item);
        //    amountList.Add(amount);
        //}

        public ItemAmount(int itemID, string itemName, ItemCategory category, int amount)
        {

            this.itemID = itemID;
            this.itemName = itemName;
            this.category = category;

            this.amount = amount;
        }

        public ItemAmount(InventoryItem inventoryItem)
        {
            amount = inventoryItem.amount;
            itemID = inventoryItem.itemID;
            itemName = inventoryItem.itemName;
            category = inventoryItem.category;

        }

    }

    public class Inventory : SingletonBase<Inventory>
    {
        private const string fileName = "inventory.dat";

        private static ItemAmount[] items;
        private static List<ItemAmount> itemsList;

        [SerializeField] private InventoryItem m_Kuklon;


        private new void Awake()
        {
            base.Awake();

            Saver<ItemAmount[]>.TryLoad(fileName, ref items);
            //FindInv();
            itemsList = items.ToList();

        }

        public static void SaveInventory()
        {
            if (Instance)
            {

                Instance.SaveItems();
                
            }
            else
            {
                Debug.Log($"Backpack= ");
            }
        }

        private void SaveItems()
        {
            items = itemsList.ToArray();
            Saver<ItemAmount[]>.Save(fileName, items);
        }

        public void AddItemInstances(InventoryItem invItem, int amount)
        {
            
            var itemAmount = FindItemByID(invItem.itemID);
            if (itemAmount == null)
            {
                itemAmount = AddItemKind(invItem);
                itemsList.Add(itemAmount);
            }
            
            
            print(itemAmount.itemName);
            itemAmount.amount += amount;
           
            
                
          
            items = itemsList.ToArray();
            
                
            Saver<ItemAmount[]>.Save(fileName, items);
        }

        public ItemAmount AddItemKind(InventoryItem invItem)
        {
            print("amount = 0");
            ItemAmount itemAmount = new ItemAmount(invItem);
            itemsList.Add(itemAmount);
            items = itemsList.ToArray();

            Saver<ItemAmount[]>.Save(fileName, items);

            return itemAmount;
        }


        public void RemoveItemInstances(InventoryItem invItem, int amount)
        {

            var itemAmount = FindItemByID(invItem.itemID);
            if (itemAmount == null)
            {
                itemAmount = AddItemKind(invItem);
                itemsList.Add(itemAmount);
            }


            print(itemAmount.itemName);
            if (itemAmount.amount >= amount)
            {
                itemAmount.amount -= amount;

                items = itemsList.ToArray();


                Saver<ItemAmount[]>.Save(fileName, items);
            }
                




        }


        public void WithdrawKuklons(int cost)
        {
            RemoveItemInstances(m_Kuklon, cost);
        }
        public void AddKuklons(int sum)
        {
            AddItemInstances(m_Kuklon, sum);
        }

        


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SaveItems();
            }
        }

      

        public int GetItemAmount(int id)
        {
            foreach (var data in items)
            {
                if (data.itemID == id)
                {
                    return data.amount;
                }
            }
            return 0;
        }

        private static ItemAmount FindItemByID(int id)
        {
            foreach (var data in items)
            {
                if (data.itemID == id)
                {
                    return data;
                }
            }
            return null;
        }

        public bool MayRemove(InventoryItem inventoryItem, int v)
        {
            var item = FindItemByID(inventoryItem.itemID);
            if (item.amount >= v)
            {
                return true;
            }
            return false;
        }
    }
}

