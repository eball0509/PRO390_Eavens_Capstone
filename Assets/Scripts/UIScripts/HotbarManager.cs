using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public const int TOTAL_HOTBAR_SLOTS = 7;

    [SerializeField] private Player player;
    [SerializeField] private InventoryObject playerInventory;
    [SerializeField] private RectTransform highlight;

    [SerializeField] private List<Ui_ItemSlot> hotbarSlots = new List<Ui_ItemSlot>();

    private int currentSlot = 0;

    private void Start()
    {
        if (player != null)
        {
            player.inventory.OnInventoryChangedCallBack += RefreshHotbar;

            InitializeHotbar();
            RefreshHotbar();
            SelectSlot(0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < TOTAL_HOTBAR_SLOTS; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }

        if (highlight != null && hotbarSlots.Count > 0)
        {
            highlight.position = Vector3.Lerp(highlight.position, hotbarSlots[currentSlot].transform.position, Time.deltaTime * 10f);
        }
    }

    private void InitializeHotbar()
    {
        for(int i = 0;i < hotbarSlots.Count ;i++)
        {
            hotbarSlots[i].slotIndex = i;

            hotbarSlots[i].player = player;
            hotbarSlots[i].assignedInventory = playerInventory;
        }
    }

    public void RefreshHotbar()
    {
        var container = player.inventory.Container;

        for (int i = 0;i < TOTAL_HOTBAR_SLOTS;i++)
        {
            if (i < hotbarSlots.Count && i < container.Count)
            {
                var inventorySlot = container[i];

                if (inventorySlot.item != null)
                {
                    hotbarSlots[i].SetItem(inventorySlot.item, inventorySlot.amount);
                }
                else
                {
                    hotbarSlots[i].ClearSlot();
                }
            }

        }
    }

    public void SelectSlot(int index)
    {
        currentSlot = index;

        Debug.Log($"Hotbar Tab Switched: Slot {index + 1} selected.");

        player.SetCurrentHotbarItem(currentSlot);

        UpdateHighlight();
    }

    public List<Ui_ItemSlot> GetHotbarSlots()
    {
        return hotbarSlots;
    }

    private void UpdateHighlight()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            var borderImg = hotbarSlots[i].transform.Find("Border").GetComponent<UnityEngine.UI.Image>();
            if (borderImg != null)
            {
                borderImg.color = (i == currentSlot) ? Color.yellow : Color.black;
            }
        }
    }
}
