using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public class StoveUI : MonoBehaviour
    {
        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }
        [SerializeField] private Inventory m_Inventory;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitializeStove();
        }
        private void InitializeStove()
        {
            for (int i = 0; i < dishIcons.Length; i++)
            {
                dishIcons[i].InitialiseSetImage();
            }

            for (int i = 0; i < ingredIcons.Length; i++)
            {
                ingredIcons[i].SetNull();
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
        
        [SerializeField] private ItemIcon[] ingredIcons;
        [SerializeField] private ItemIcon resultIcon;

        [SerializeField] private ItemIcon[] dishIcons;

        public void Cook(CraftRecipe recipe)
        {

            InitializeStove();

            bool canCook = true;

            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                canCook &= m_Inventory.MayRemove(recipe.ingredients[i], recipe.amounts[i]);

                ingredIcons[i].InitialiseSetItem(recipe.ingredients[i]);
            }
            
            resultIcon.InitialiseSetItem(recipe.result);

            if (canCook)
            {
                for (int i = 0; i < recipe.ingredients.Length; i++)
                {
                    m_Inventory.AddItemInstances(recipe.ingredients[i], -recipe.amounts[i]);
                }

                m_Inventory.AddItemInstances(recipe.result, 1);

                InventoryController.Instance.InitAllItems();

                
            }
        }
    }

}
