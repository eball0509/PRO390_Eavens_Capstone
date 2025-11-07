using UnityEngine;

[System.Serializable]
public class CraftingIngredient
{ 
    public ItemObject item;
    public int amount;
}




[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public CraftingIngredient[] ingredients;
    public ItemObject result;
    public int resultAmount = 1;
}
