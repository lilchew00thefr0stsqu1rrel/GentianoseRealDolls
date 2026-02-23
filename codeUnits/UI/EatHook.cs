using UnityEngine;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(ItemIcon))]
    public class EatHook : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_ItemIcon = GetComponent<ItemIcon>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private ItemIcon m_ItemIcon;
        
        public void EatFood()
        {
            Dashboard.Instance.Eat(m_ItemIcon.InventoryItem);
        }
    }
}

