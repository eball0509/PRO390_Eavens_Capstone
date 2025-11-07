using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public int damage;
    public int yieldMultiplier;
    public GameObject prefab;

    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    private void Awake()
    {
        type = ItemType.Tool;
    }
}
