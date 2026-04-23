
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private InventoryObject inventoryTemplate;
    [SerializeField] private int chestSize = 15;
    public InventoryObject inventory;

    private void Awake()
    {
        inventory = Instantiate(inventoryTemplate);
        inventory.Initialize(chestSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            InventoryUI.Instance.OpenChest(inventory);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            InventoryUI.Instance.CloseChest();
    }
}