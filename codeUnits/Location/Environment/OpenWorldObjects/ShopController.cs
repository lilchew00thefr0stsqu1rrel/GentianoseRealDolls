using UnityEngine;
using UnityEngine.UI;
namespace GentianoseRealDolls
{
    public class ShopController : MonoBehaviour
    {
        private int m_Kuklons;
        [SerializeField] private Text m_KuklonsIHave;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Kuklons = Inventory.Instance.GetItemAmount(2);
            m_KuklonsIHave.text = m_Kuklons.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);    
            }
        }

        [SerializeField] private InventoryItem[] m_Goods;
        [SerializeField] private int[] m_PriceArray;

        public void Buy(int itemIndex)
        {
            Inventory.Instance.AddItemInstances(m_Goods[itemIndex], 1);

            Inventory.Instance.WithdrawKuklons(m_PriceArray[itemIndex]);

            m_Kuklons = Inventory.Instance.GetItemAmount(2);
            m_KuklonsIHave.text = m_Kuklons.ToString();

            InventoryController.Instance.InitAllItems();
        }
    }
}

