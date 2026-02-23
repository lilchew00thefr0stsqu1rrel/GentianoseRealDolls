using UnityEngine;

namespace GentianoseRealDolls
{
    public class InventoryController : SingletonBase<InventoryController>
    {
        [SerializeField] private ItemIcon[] items;

        //[SerializeField] private ItemAmount[] itemRecords;

        [SerializeField] private InventoryItem itemRecord;
        
        private void Start()
        {
            print("showing levels");
            foreach (var item in items)
            {
                item.Initialise();
                print(item.name);
            }
            
        }

        //[SerializeField] private InventoryItem testItem;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Inventory.Instance.AddItemInstances(itemRecord, 2);
                Inventory.SaveInventory();
                InitAllItems();
            }
        }

        public void InitAllItems()
        {
            foreach (var item in items)
            {
                item.Initialise();
                print(item.name);
            }
        }
        
       
    }
}

