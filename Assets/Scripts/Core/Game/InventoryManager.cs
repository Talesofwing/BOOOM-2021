using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Kuma.Utils.Singleton;
using Kuma.Utils.UpdateManager;
using System.Linq;

public class InventoryManager : MonoSingleton<InventoryManager>, ILateUpdatable
{
    public const int InventorySize = 20;
    public int CurrentEquip
    {
        get
        {
            if(IsSlotEmpty(_selectedSlot))
            {
                return -1;
            }
            return slotList[_selectedSlot].Item.ID;
        }
    }
    private int _selectedSlot;
    

    private InventorySlot[] slotList;
    private UIInventoryHud _inventoryUI;

    public event Action<int, int> onEquipChanged;

    public void Init() 
    {
        slotList = new InventorySlot[InventorySize];
        for (int i = 0; i < InventorySize; i++)
        {
            slotList[i] = new InventorySlot();
        }
        _selectedSlot = 0;
        
        UpdateDispatcher.Instance.AddLateUpdatable(this); 
    }

    public void Load(bool isNewGame)
    {
        if(!isNewGame)
        {
            LoadSaveData(SaveManager.GetInventorySave());
        }

    }

    public void SetInventoryUI(UIInventoryHud ui)
    {
        _inventoryUI = ui;
    }
    
    public void CallLateUpdate(float deltaTime)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if(slotList[i].requireRefresh)
            {
                _inventoryUI.RefreshSlot(i, slotList[i].Item, slotList[i].Count);
                slotList[i].requireRefresh = false;
            }
        }
    }

    public bool CanTakeItem(int itemId, int count) => TryTakeItem(itemId, count, preview: true);

    public bool TryTakeItem(int itemId, int count, bool preview = false)
    {
        var itemData = DataManager.GetItemData(itemId);
        var maxStack = itemData.MaxStack;
        var baseCount = count;

        //stack Item        
        for (int i = 0; i < InventorySize; i++)
        {
            if(slotList[i].IsSame(itemId))
            {
                var canTake = maxStack - slotList[i].Count;
                var takeCount = Mathf.Min(count, canTake);
                if(!preview)
                {
                    slotList[i].Count += takeCount;
                }
                count -= takeCount;
                if(count <= 0)
                {
                    AudioManager.Instance.PlaySE("item0" + (itemData.SoundID - 1));
                    return true;
                }
            }
        }

        //AddNewSlot
        for (int i = 0; i < InventorySize; i++)
        {
            if(IsSlotEmpty(i))
            {
                var takeCount = Mathf.Min(count, maxStack);
                if(!preview)
                {
                    slotList[i].Item = itemData;           
                    slotList[i].Count = takeCount;

                    if(i == _selectedSlot && ! slotList[i].IsEmpty)
                    {
                        onEquipChanged(CurrentEquip, _selectedSlot);
                    }
                }
                count -= takeCount;
                if(count <= 0)
                {
                    AudioManager.Instance.PlaySE("item0" + UnityEngine.Random.Range(0, 4));
                    return true;
                }
            }
        }

        return count < baseCount;
    }

    public bool CanCostItem(int itemId, int count) => TryCostItem(itemId, count, preview: true);
    public bool TryCostItem(int itemId, int count, bool preview = false)
    {
        if(GetItemCount(itemId) < count)
        {
            Debug.Log("Item[" + itemId + "] not enough");
            return false;
        }
        else if(preview)
        {
            return true;
        }

        if(slotList[_selectedSlot].IsSame(itemId))
        {
            var cost = Mathf.Min(count , slotList[_selectedSlot].Count);
            slotList[_selectedSlot].Count -= cost;
            count -= cost;
            if(slotList[_selectedSlot].IsEmpty)
            {
                onEquipChanged(CurrentEquip, _selectedSlot);
            }
        }

        if(count <= 0)
        {
            return true;
        }

        for (int i = 0; i < InventorySize; i++)
        {            
            if(slotList[i].IsSame(itemId))
            {
                var cost = Mathf.Min(count , slotList[i].Count);
                slotList[i].Count -= cost;
                count -= cost;
                if(i == _selectedSlot && slotList[i].IsEmpty)
                {
                    onEquipChanged(CurrentEquip, _selectedSlot);
                }

                if(count <= 0)
                {
                    return true;
                }
            }
        }

        Debug.Log("Item Count Check Error");
        return false;
    }

    public int GetItemCount(int itemId)
    {
        var count = 0;

        foreach (var slot in slotList)
        {
            if(slot.IsSame(itemId))
            {
                count += slot.Count;
            }
        }

        return count;
    }

    public void DeleteSlot(int slotId)
    {
        slotList[slotId].Clear();
    }

    //equipment
    public void SelectEquip(int slotId) {
        _selectedSlot = slotId;

        if (onEquipChanged != null)
            onEquipChanged(CurrentEquip, _selectedSlot);
    }
    
    public bool IsSlotEmpty(int slotId)
    {
        return slotList[slotId].IsEmpty;
    }

    public int GetSlotItemID(int slotId)
    {
        return slotList[slotId].ItemID;
    }

    public bool TryShowCraftMenu(int slotId)
    {
        if(IsSlotEmpty(slotId))
        {
            return false;
        }

        return CraftManager.Instance.TryShowCraftMenu(slotList[slotId].Item);
    }

    public int[] ConvertToSaveDataId()
    {
        return slotList.Select(slot => slot.ItemID).ToArray();
    }

    public int[] ConvertToSaveDataCount()
    {
        return slotList.Select(slot => slot.Count).ToArray();
    }

    public void ConvertFormSaveData(int[] ids, int[] count)
    {
        for (int i = 0; i < slotList.Length; i++)
        {
            slotList[i].Item = DataManager.GetItemData(ids[i]);
            slotList[i].Count = count[i];
        }
    }

    public InventoryData GetInventorySave()
    {
        return new InventoryData()
        {
            itemIds = ConvertToSaveDataId(),
            itemCounts = ConvertToSaveDataCount(),
        };
    }

    public void LoadSaveData(InventoryData inventorySave)
    {
        ConvertFormSaveData(inventorySave.itemIds, inventorySave.itemCounts);
    }


    private struct InventorySlot
    {
        private ItemData _item;
        public ItemData Item
        {
            get => _item;
            set
            {
                _item = value;
                requireRefresh = true;
                if(_item != null)
                {
                    CraftManager.Instance.OnGetItem(_item.ID);
                }
            }
        }

        public int ItemID
        {
            get
            {
                if(IsEmpty)
                {
                    return -1;
                }
                else
                {
                    return Item.ID;
                }
            }
        }
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                requireRefresh = true;
            }
        }

        public bool IsEmpty => Item == null || Count == 0;

        public bool requireRefresh;

        public bool IsSame(int index) => !IsEmpty && Item.ID == index;

        public bool CanStack(int index)
        {
            return IsEmpty || Item.ID == index;
        }

        public void Clear()
        {
            Item = null;
            Count = 0;
        }

    }
}
