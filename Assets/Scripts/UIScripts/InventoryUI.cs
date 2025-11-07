using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryObject playerInventory;
    [SerializeField] private Player player;
    [SerializeField] private List<Ui_ItemSlot> uiSlots = new();
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private HotbarManager hotbarManager;

    private GameObject currentPanel;
    private bool isVisible = false;


    private void Start()
    {
        playerInventory.Initialize(28);
        InitializeMainSlots();

        playerInventory.OnInventoryChangedCallBack += RefreshUI;
        RefreshUI();

        pauseMenuPanel.SetActive(false);
        HideAllPanels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }
    }

    private void OnDisable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChangedCallBack -= RefreshUI;
        }
    }

    public void RefreshUI()
    {
        if (playerInventory == null) return;

        var container = playerInventory.Container;
        int startOffset = HotbarManager.TOTAL_HOTBAR_SLOTS;

        for (int i = 0; i < uiSlots.Count; i++)
        {
            int inventoryIndex = i + startOffset;

            if (inventoryIndex < container.Count)
            {
                var inventorySlot = container[inventoryIndex];

                if (inventorySlot.item != null)
                {
                    uiSlots[i].SetItem(inventorySlot.item, inventorySlot.amount);
                }
                else
                {
                    uiSlots[i].ClearSlot();
                }
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
        hotbarManager.RefreshHotbar();
    }

    public void ToggleInventory()
    {
        isVisible = !isVisible;
        pauseMenuPanel.SetActive(isVisible);

        if (isVisible)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ShowPanel(inventoryPanel);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void InitializeMainSlots()
    {
        int startOffset = HotbarManager.TOTAL_HOTBAR_SLOTS;

        for(int i = 0;i < uiSlots.Count;i++)
        {
            uiSlots[i].player = player;
            uiSlots[i].slotIndex = i + startOffset;

            uiSlots[i].assignedInventory = playerInventory;
        }
    }

    private void HideAllPanels()
    {
        if (inventoryPanel) inventoryPanel.SetActive(false);
        if (craftingPanel) craftingPanel.SetActive(false);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            currentPanel = panelToShow;
        }

    }

    public void OpenInventory() => ShowPanel(inventoryPanel);
    public void OpenCrafting() => ShowPanel(craftingPanel);
}
