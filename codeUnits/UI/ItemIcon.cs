using GentianoseRealDolls;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ItemIcon : MonoBehaviour
{

    [Inject]
    public void Construct(Inventory obj)
    {
        m_Inventory = obj;
    }
    [SerializeField] private Inventory m_Inventory;

    [SerializeField] private InventoryItem inventoryItem;
    public InventoryItem InventoryItem => inventoryItem;
    [SerializeField] private Text amountText; 

    public void Initialise()
    {
        var amount = m_Inventory.GetItemAmount(inventoryItem.itemID);
        gameObject.SetActive(amount > 0);
       
        amountText.text = amount.ToString();
        
        print("Amount " + amount + " ITEmID "+ inventoryItem.itemID);
       // print("Init " + inventoryItem.itemID + "  " + amount);

        //Inventory.SaveInventory(inventoryItem, amount);
    }

    [SerializeField] private Image image;


   [SerializeField]  private int m_Amount;
    public void InitialiseSetItem(InventoryItem item)
    {
        inventoryItem = item;   
        image.sprite = item.icon;

        var amount = m_Inventory.GetItemAmount(inventoryItem.itemID);
      
        m_Amount = amount;
        amountText.text = amount.ToString();

        // print("Init " + inventoryItem.itemID + "  " + amount);

        //Inventory.SaveInventory(inventoryItem, amount);
    }
    public void InitialiseSetImage()
    {
        image.sprite = inventoryItem.icon;

        var amount = m_Inventory.GetItemAmount(inventoryItem.itemID);
        

        amountText.text = amount.ToString();

    }

    public void SetNull()
    {
        image.sprite = null;
        amountText.text = "---";
    }
}
