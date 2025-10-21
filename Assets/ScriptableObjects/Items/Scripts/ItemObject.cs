using UnityEngine;


public enum ItemType
{
    Resource,
    Material,
    Tool,
    Food
}

public abstract class ItemObject : ScriptableObject
{
    //public GameObject prefab;
    public ItemType type;

    [TextArea(15, 20)]
    public string description;

}
