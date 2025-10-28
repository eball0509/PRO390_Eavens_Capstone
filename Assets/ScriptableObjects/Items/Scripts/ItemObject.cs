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
    public ItemType type;
    public Sprite icon;

    [TextArea(15, 20)]
    public string description;

}