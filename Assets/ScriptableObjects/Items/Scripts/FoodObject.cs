using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    [Description("Attributes")]
    public int healthRestore;
    public int hungerRestore;

    private void Awake()
    {
        type = ItemType.Food;
    }
}
