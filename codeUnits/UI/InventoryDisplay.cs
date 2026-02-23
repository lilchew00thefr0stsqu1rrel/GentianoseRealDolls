using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GentianoseRealDolls
{
    public class InventoryDisplay : MonoBehaviour
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


            
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape)) 
            //{ 
            //    inventoryUI.SetActive(false);
            //}
        }
    }

}
