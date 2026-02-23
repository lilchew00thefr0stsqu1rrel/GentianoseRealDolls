using GentianoseRealDolls;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    

    [SerializeField] private InventoryItem inventoryItem;
    public InventoryItem InventoryItem => inventoryItem;
    [SerializeField] private Text amountText; 

    public void Initialise()
    {
        var amount = Inventory.Instance.GetItemAmount(inventoryItem.itemID);
        gameObject.SetActive(amount > 0);
       
        amountText.text = amount.ToString();    

       // print("Init " + inventoryItem.itemID + "  " + amount);

        //Inventory.SaveInventory(inventoryItem, amount);
    }
}
