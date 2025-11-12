using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName ="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public delegate void OnInventoryChanged();

    public event OnInventoryChanged OnInventoryChangedCallBack;

    public void Initialize(int size)
    {
        if (Container.Count == 0)
        {
            for (int i = 0; i < size; i++)
            {
                Container.Add(new InventorySlot());
            }
        }

    }

    public void AddItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmout(_amount);
                OnInventoryChangedCallBack?.Invoke();
                return;
            }
        }
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == null)
            {
                Container[i].UpdateSlot(_item, _amount);
                OnInventoryChangedCallBack?.Invoke();
                return;
            }
        }

    }

    public void SwapItems(int indexA, int indexB)
    {
        Debug.Log($"Attempting swap from {indexA} to {indexB}"); // Check 1: Is this line executed?
        InventorySlot temp = Container[indexA];
        Container[indexA] = Container[indexB];
        Container[indexB] = temp;
        Debug.Log($"Swap successful between {indexA} and {indexB}"); // Check 2: Is this line executed?

        OnInventoryChangedCallBack?.Invoke();
    }

    public void NotifyChange()
    {
       OnInventoryChangedCallBack?.Invoke();
    }

    public void RemoveItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Count && _amount > 0; i++)
        {
            InventorySlot slot = Container[i];

            if (slot.item == _item)
            {
                int remove = Mathf.Min(slot.amount, _amount);
                slot.amount -= remove;
                _amount -= remove;

                if (slot.amount <= 0)
                {
                    slot.UpdateSlot(null, 0);
                }
            }
        }

        OnInventoryChangedCallBack?.Invoke();
    }

}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot()
    {
        item = null;
        amount = 0;
    }


    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmout(int value)
    {
        amount += value;
    }

}

