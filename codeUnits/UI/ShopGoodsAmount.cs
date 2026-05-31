using GentianoseRealDolls;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopGoodsAmount : MonoBehaviour
{
    [Inject]
    public void Construct(Inventory obj)
    {
        m_Inventory = obj;
    }
    [SerializeField] private Inventory m_Inventory;

    [SerializeField] private InventoryItem m_GoodsItem;
    [SerializeField] private Text m_AmountText;
    [SerializeField] private Image m_Icon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Icon.sprite = m_GoodsItem.icon;
        UpdateTextValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: Observer
    public void UpdateTextValue()
    {
        m_AmountText.text = $"� �������: {m_Inventory.GetItemAmount(m_GoodsItem.itemID)} ������";
    }
}
