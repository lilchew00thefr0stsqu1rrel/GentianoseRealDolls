using GentianoseRealDolls;
using UnityEngine;

[SerializeField]
[CreateAssetMenu]
public class CraftRecipe : ScriptableObject
{
    public InventoryItem[] ingredients;
    public int[] amounts;
    public InventoryItem result;


}
