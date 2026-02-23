using UnityEngine;

namespace GentianoseRealDolls
{
    public class StoveUI : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
        }


        public void Cook(CraftRecipe recipe)
        {
            bool canCook = true;
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                
                canCook &= Inventory.Instance.MayRemove(recipe.ingredients[i], recipe.amounts[i]);
                
                
            }
            if (canCook)
            {
                for (int i = 0; i < recipe.ingredients.Length; i++)
                {
                    Inventory.Instance.AddItemInstances(recipe.ingredients[i], -recipe.amounts[i]);
                }

                Inventory.Instance.AddItemInstances(recipe.result, 1);

                InventoryController.Instance.InitAllItems();
            }
        }
    }

}
