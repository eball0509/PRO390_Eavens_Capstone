using UnityEngine;

[CreateAssetMenu(fileName = "New Material Object", menuName = "Inventory System/Items/Material")]
public class MaterialObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Material;
    }
}
