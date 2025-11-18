using UnityEngine;

[CreateAssetMenu(fileName = "AnimalDropTable", menuName = "Items/Animal Drop Table")]
public class AnimalDropTable : ScriptableObject
{
    [System.Serializable]
    public struct AnimalDrops
    {
        public ItemObject item;
        public int amount;
    }

    public AnimalDrops[] drops;
}
