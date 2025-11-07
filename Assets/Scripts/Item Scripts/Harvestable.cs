using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public ItemObject itemToGive;
    public int amtToGivePerHit = 3;
    public int currentHealth;
    public int maxHealth = 5;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Harvest(Player player, ToolObject tool)
    {
        int appliedDamage = tool != null ? tool.damage : 1;
        currentHealth-= appliedDamage;

        int yield = tool != null ? amtToGivePerHit * tool.yieldMultiplier : amtToGivePerHit;
        player.inventory.AddItem(itemToGive, yield);
        Debug.Log($"Gave {yield}x {itemToGive.name} to player!");


        if (currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
