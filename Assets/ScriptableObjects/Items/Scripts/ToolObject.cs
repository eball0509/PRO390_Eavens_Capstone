using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public int damage;
    public int yieldMultiplier;
    public GameObject prefab;
    public string useAnimationTrigger = "Swing";

    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    public AudioClip[] woodHitSounds;
    public AudioClip[] stoneHitSounds;
    public AudioClip[] fleshHitSounds;
    public AudioClip[] defaultHitSounds;

    private void Awake()
    {
        type = ItemType.Tool;
    }
}
