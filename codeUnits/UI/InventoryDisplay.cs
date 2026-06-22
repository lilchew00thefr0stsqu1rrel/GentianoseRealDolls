using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GentianoseRealDolls
{
    public class InventoryDisplay : DashboardBase
    {
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private GameObject foodItemGrid;

        [SerializeField] private GameObject inventoryUI;

        [SerializeField] private List<InventoryItem> inventoryItems;

        private List<int> inventoryAmount;

        private void Start()
        {
            inventoryItems = new List<InventoryItem>();
            inventoryAmount = new List<int>();

            gameObject.SetActive(false);
            
        }


        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape)) 
            //{ 
            //    inventoryUI.SetActive(false);
            //}
        }

        private void OnEnable()
        {
            InventoryController.Instance.InitAllItems();
        }
    }

}
