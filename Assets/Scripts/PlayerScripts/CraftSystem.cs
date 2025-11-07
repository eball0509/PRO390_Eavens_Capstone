using UnityEngine;

public class CraftSystem : MonoBehaviour
{
    public bool TryCraft(CraftingRecipe recipe, Player player)
    {
        InventoryObject inventory = player.inventory;

        foreach (var ingredient in recipe.ingredients)
        {
            RemoveItem(inventory, ingredient.item, ingredient.amount);
        }

        inventory.AddItem(recipe.result, recipe.resultAmount);

        inventory.NotifyChange();

        return true;
    }
    

    private bool HasIngredients(InventoryObject inventory, CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            int totalInInventory = 0;

            foreach (var slot in inventory.Container)
            {
                if (slot.item == ingredient.item)
                {
                    totalInInventory += slot.amount;
                }
            }
            if (totalInInventory < ingredient.amount)
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveItem(InventoryObject inventory, ItemObject item, int amountToRemove)
    {
        for (int i = 0; i < inventory.Container.Count && amountToRemove > 0; i++)
        {
            InventorySlot slot = inventory.Container[i];

            if (slot.item == item)
            {
                int remove = Mathf.Min(slot.amount, amountToRemove);
                slot.amount -= remove;
                amountToRemove -= remove;

                if (slot.amount <= 0)
                {
                    slot.UpdateSlot(null, 0);
                }
            }
        }
    }

    public bool PlayerHasIngredients(InventoryObject inventory, CraftingRecipe recipe)
    {
        return HasIngredients(inventory, recipe);
    }


}
