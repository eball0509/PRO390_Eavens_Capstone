using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ui_ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int slotIndex;
    public Player player;
    public static Transform rootCanvasTransform;
    public InventoryObject assignedInventory;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    private ItemObject currentItem;
    private int currentAmount;

    public static Ui_ItemSlot itemBeingDragged;
    private RectTransform iconRectTransform;
    private CanvasGroup iconCanvasGroup;

    private void Awake()
    {
        if (icon == null)
            icon = GetComponentInChildren<Image>();
        if (amountText == null)
            amountText = GetComponentInChildren<TMP_Text>();

        iconRectTransform = icon.GetComponent<RectTransform>();

        // 🆕 Add/Grab CanvasGroup for drag raycast handling
        iconCanvasGroup = icon.GetComponent<CanvasGroup>();
        if (iconCanvasGroup == null)
            iconCanvasGroup = icon.gameObject.AddComponent<CanvasGroup>();
    }

    public void SetItem(ItemObject item, int amount)
    {
        currentItem = item;
        currentAmount = amount;

        if (item != null && item.icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
            icon.color = Color.white;
            amountText.text = amount > 1 ? amount.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentAmount = 0;
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag clicked on Slot Index: {slotIndex}");
        if (player.inventory.Container[slotIndex].item == null)
            return;

        Debug.Log($"Starting drag from Slot Index: {slotIndex}");

        itemBeingDragged = this;

        if (rootCanvasTransform == null)
        {
            Debug.LogError("rootCanvasTransform is NULL! Ensure Canvas is assigned correctly.");
            return;
        }

        iconRectTransform.SetParent(rootCanvasTransform);

        icon.raycastTarget = false;

        if (iconCanvasGroup != null)
            iconCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemBeingDragged != null)
            iconRectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag Fired on Slot: {slotIndex}");

        if (itemBeingDragged == this)
            ResetDragVisuals();

        itemBeingDragged = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDrop fired on Slot Index: {this.slotIndex}");

        if (itemBeingDragged == null || itemBeingDragged == this)
            return;

        Debug.Log($"Valid drop from {itemBeingDragged.slotIndex} to {this.slotIndex}");

        Ui_ItemSlot sourceSlot = itemBeingDragged;

        if (sourceSlot.assignedInventory == assignedInventory)
            assignedInventory.SwapItems(sourceSlot.slotIndex, this.slotIndex);

        sourceSlot.ResetDragVisuals();
        itemBeingDragged = null;
    }

    private void ResetDragVisuals()
    {
        icon.raycastTarget = true;

        if (iconCanvasGroup != null)
            iconCanvasGroup.blocksRaycasts = true;

        iconRectTransform.SetParent(transform);
        iconRectTransform.localPosition = Vector3.zero;
    }
}