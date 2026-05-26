using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using UnityEngine;


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

    public class Inventory : MonoBehaviour
    {
        private string fileName;
        private string path;

        [SerializeField] private ItemAmount[] m_Items;
        [SerializeField] private List<ItemAmount> m_ItemsList;

        [SerializeField] private InventoryItem m_Kuklon;

        [SerializeField] private List<int> m_ItemsMap;
        [SerializeField] private UnityEngine.UI.Text m_DebugText;

        [SerializeField] private DollBase m_DollBase;


        Action<InventoryItem, int> TakeResource(InventoryItem item, int amount)
        {
            return (item, amount) =>
            {
                AddItemInstances(item, amount);
            };
        }
        private InventoryItem baseItem;
        private int baseAmount;

        

        public void InitInventory()
        {
            GiveResource.OnTake += TakeResource(baseItem, baseAmount); 

            Saver<List<int>>.TryLoad(WhooSettings.fileNameInv, ref m_ItemsMap);



            m_ItemsMap = m_DollBase.GetItemAmounts().ToList();

            //FindInv();

            //if (m_Items.Length == 0 || m_Items != null)
            //{
            //    m_Items = new ItemAmount[16];
            //    Saver<List<int>>.Save(WhooSettings.fileNameInv, m_ItemsMap);
            //}

            //m_ItemsList = m_Items.ToList();

            if (m_ItemsMap == null || m_ItemsMap.Count == 0)
                m_ItemsMap = new List<int>(16);


            //for (int i = 0; i < m_ItemsList.Count; i++)
            //{
            //    m_ItemsMap[i] = m_ItemsList[i].amount;
            //}

            InventoryController.Instance.InitAllItems();
        }

        

        private void Awake()
        {
            //Saver<ItemAmount[]>.TryLoad(WhooSettings.fileNameInv, ref m_Items);
            ////FindInv();

            //if (m_Items != null)
            //{
            //    m_ItemsList = m_Items.ToList();
            //}
            //else
            //{
            //    m_ItemsList = new List<ItemAmount>();
            //}
        }
        private void OnApplicationPause(bool pause)
        {
            GiveResource.OnTake -= TakeResource(baseItem, baseAmount);
        }

        private void OnDestroy()
        {
            GiveResource.OnTake -= TakeResource(baseItem, baseAmount);
        }
        public void SaveInventory()
        {
            SaveItems();
        }

        private void SaveItems()
        {
            m_Items = m_ItemsList.ToArray();
            Saver<List<int>>.Save(WhooSettings.fileNameInv, m_ItemsMap);
        }

        public void AddItemInstances(InventoryItem invItem, int amount)
        {
            
            //var itemAmount = FindItemByID(invItem.itemID);
            //if (itemAmount == null)
            //{
            //    itemAmount = AddItemKind(invItem);
            //    m_ItemsList.Add(itemAmount);
            //}
            
            
            //print(itemAmount.itemName);
            //itemAmount.amount += amount;

            


            m_ItemsMap[invItem.itemID] += amount;
            


            //m_Items = m_ItemsList.ToArray();

            m_DollBase.ChangeItemAmount(invItem.itemID, m_ItemsMap[invItem.itemID]);

            Saver<List<int>>.Save(WhooSettings.fileNameInv, m_ItemsMap);
        }

        public ItemAmount AddItemKind(InventoryItem invItem)
        {
            print("amount = 0");
            ItemAmount itemAmount = new ItemAmount(invItem);
            m_ItemsList.Add(itemAmount);
            m_Items = m_ItemsList.ToArray();

            Saver<List<int>>.Save(WhooSettings.fileNameInv, m_ItemsMap);



            return itemAmount;
        }


        public void RemoveItemInstances(InventoryItem invItem, int amount)
        {

            //var itemAmount = FindItemByID(invItem.itemID);
            //if (itemAmount == null)
            //{
            //    itemAmount = AddItemKind(invItem);
            //    m_ItemsList.Add(itemAmount);
            //}


            //print(itemAmount.itemName);
            //if (itemAmount.amount >= amount)
            //{
            //    itemAmount.amount -= amount;

            //    m_Items = m_ItemsList.ToArray();


            //}

            if (m_ItemsMap[invItem.itemID] >= amount)
            {
                m_ItemsMap[invItem.itemID] -= amount;
            }

            
            m_DollBase.ChangeItemAmount(invItem.itemID, m_ItemsMap[invItem.itemID]);

            Saver<List<int>>.Save(WhooSettings.fileNameInv, m_ItemsMap);

        }


        public void WithdrawKuklons(int cost)
        {
            RemoveItemInstances(m_Kuklon, cost);
        }
        public void AddKuklons(int sum)
        {
            AddItemInstances(m_Kuklon, sum);
        }





        public int GetItemAmount(int id)
        {

            //Saver<ItemAmount[]>.TryLoad(WhooSettings.fileNameInv, ref m_Items);
            //FindInv();
            //if (m_Items != null)
            //{
            //    m_ItemsList = m_Items.ToList();

            //    foreach (var data in m_Items)
            //    {
            //        if (data != null) return 0;
            //        if (data.itemID == id)
            //        {
            //            return data.amount;
            //        }
            //    }
            //}
            if (m_ItemsMap != null && m_ItemsMap.Count > id)
            {
                return m_ItemsMap[id];
            }
            else
            {
                m_ItemsList = new List<ItemAmount>();
            }
            print("***");
            return 0;
        }

        private ItemAmount FindItemByID(int id)
        {
            foreach (var data in m_Items)
            {
                if (data.itemID == id)
                {
                    return data;
                }
            }
            return null;
        }

        private int GetItemAmountByID(int id)
        {
            if (m_ItemsMap.Count > id)
                return m_ItemsMap[id];
                else
                return 0;
        }

        public bool MayRemove(InventoryItem inventoryItem, int v)
        {
            var item = GetItemAmountByID(inventoryItem.itemID);
            if (item >= v)
            {
                return true;
            }
            return false;
        }
    }
}

