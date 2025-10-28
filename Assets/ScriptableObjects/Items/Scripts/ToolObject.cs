using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public int damage;
    public int yieldMultiplier;

    private void Awake()
    {
        type = ItemType.Tool;
    }
}
