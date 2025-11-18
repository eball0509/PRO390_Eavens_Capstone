using UnityEngine;

public class AnimalHarvestable : MonoBehaviour
{
    public AnimalDropTable droptable;

    public void Harvest(Player player, ToolObject tool)
    {
        foreach (var drop in droptable.drops)
        {
            player.inventory.AddItem(drop.item, drop.amount);
        }

        Destroy(gameObject);
    }
}
