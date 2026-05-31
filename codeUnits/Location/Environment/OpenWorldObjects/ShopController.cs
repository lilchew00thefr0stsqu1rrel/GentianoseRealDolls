using UnityEngine;
using UnityEngine.UI;
using VContainer;
namespace GentianoseRealDolls
{
    public class ShopController : MonoBehaviour
    {
        private int m_Kuklons;
        [SerializeField] private Text m_KuklonsIHave;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Kuklons = m_Inventory.GetItemAmount(2);
            m_KuklonsIHave.text = m_Kuklons.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }

        [SerializeField] private Inventory m_Inventory;
        [SerializeField] private InventoryItem[] m_Goods;
        [SerializeField] private int[] m_PriceArray;

        [SerializeField] private ShopGoodsAmount[] m_GoodsDisplay;

        public void Buy(int itemIndex)
        {
            m_Inventory.AddItemInstances(m_Goods[itemIndex], 1);

            m_Inventory.WithdrawKuklons(m_PriceArray[itemIndex]);

            m_Kuklons = m_Inventory.GetItemAmount(2);
            m_KuklonsIHave.text = m_Kuklons.ToString();

            InventoryController.Instance.InitAllItems();

            for (int i = 0; i < m_GoodsDisplay.Length; i++)
            {

                m_GoodsDisplay[i].UpdateTextValue();

            }
        }
    }
}

