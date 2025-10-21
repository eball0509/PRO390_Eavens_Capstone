using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public int damage;
    private void Awake()
    {
        type = ItemType.Tool;
    }
}
