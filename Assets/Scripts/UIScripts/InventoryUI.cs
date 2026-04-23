
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [SerializeField] private InventoryObject playerInventory;
    [SerializeField] private Player player;
    [SerializeField] private List<Ui_ItemSlot> uiSlots = new();
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private HotbarManager hotbarManager;

    [Header("Chest")]
    [SerializeField] private GameObject chestPanel;
    [SerializeField] private List<Ui_ItemSlot> chestUiSlots = new();
    private InventoryObject currentChestInventory;

    private GameObject currentPanel;
    private bool isVisible = false;

    private void Awake()
    {
        Instance = this;
    }

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
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleInventory();
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryChangedCallBack -= RefreshUI;

        if (currentChestInventory != null)
            currentChestInventory.OnInventoryChangedCallBack -= RefreshChestUI;
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
                var slot = container[inventoryIndex];
                if (slot.item != null)
                    uiSlots[i].SetItem(slot.item, slot.amount);
                else
                    uiSlots[i].ClearSlot();
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
            CloseChest();
        }
    }

    private void InitializeMainSlots()
    {
        int startOffset = HotbarManager.TOTAL_HOTBAR_SLOTS;
        for (int i = 0; i < uiSlots.Count; i++)
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
        if (chestPanel) chestPanel.SetActive(false);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            currentPanel = panelToShow;
        }
    }

    public void OpenInventory() => ShowPanel(inventoryPanel);
    public void OpenCrafting() => ShowPanel(craftingPanel);


    public void OpenChest(InventoryObject chestInventory)
    {
        if (currentChestInventory != null)
            currentChestInventory.OnInventoryChangedCallBack -= RefreshChestUI;

        currentChestInventory = chestInventory;
        currentChestInventory.OnInventoryChangedCallBack += RefreshChestUI;

        for (int i = 0; i < chestUiSlots.Count; i++)
        {
            chestUiSlots[i].player = player;
            chestUiSlots[i].slotIndex = i;
            chestUiSlots[i].assignedInventory = currentChestInventory;
        }

        if (!isVisible)
            ToggleInventory();

        ShowPanel(inventoryPanel);
        chestPanel.SetActive(true);

        RefreshChestUI();
    }

    public void CloseChest()
    {
        if (currentChestInventory != null)
        {
            currentChestInventory.OnInventoryChangedCallBack -= RefreshChestUI;
            currentChestInventory = null;
        }

        if (chestPanel) chestPanel.SetActive(false);
    }

    private void RefreshChestUI()
    {
        if (currentChestInventory == null) return;
        var container = currentChestInventory.Container;

        for (int i = 0; i < chestUiSlots.Count; i++)
        {
            if (i < container.Count && container[i].item != null)
                chestUiSlots[i].SetItem(container[i].item, container[i].amount);
            else
                chestUiSlots[i].ClearSlot();
        }
    }
}