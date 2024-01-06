using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kuma.Utils.UpdateManager;
using UnityEngine.UI;

public class UIInventoryHud : BaseUI
{
    public override UIType GetUIType () {
        return UIType.Inventory;
    }
    
    [SerializeField]private UIInventoryItemSlot[] slotList;
    [SerializeField]private Transform selectMenu; 
    [SerializeField]private Button craftlistButton; 

    private int _currentSelect = -1;

    private int _menuSelect;

    private bool _inited;
    
    public override void Load (params object[] args) {
        base.Load (args);
        
        if(!_inited)
        {
            InitSlot();
        }

        if (_currentSelect != -1) {
            slotList[_currentSelect].SetSelect(false);
        }
        
        _currentSelect = 0;
        slotList[_currentSelect].SetSelect(true);
    }

    public override void Open () {
        base.Open ();
        
        InventoryManager.Instance.onEquipChanged += OnEquipChanged;
        GameManager.Instance.GameStatusChangedEvent += OnGameStatusChanged;
    }

    public override void Close () {
        base.Close ();
        
        InventoryManager.Instance.onEquipChanged -= OnEquipChanged;
        GameManager.Instance.GameStatusChangedEvent -= OnGameStatusChanged;
    }

    private void InitSlot()
    {
        if(_inited)
        {
            return;
        }
        for (int id = 0; id < slotList.Length; id++)
        {
            slotList[id].Init(this, id);
            if(id < 9)
            {
                slotList[id].SetKeycode((id + 1).ToString());
            }
            else if(id == 9)
            {
                slotList[id].SetKeycode("0");
            }
            else
            {
                slotList[id].SetKeycode(string.Empty);
            }
        }
        _inited = true;
    }

    private void OnGameStatusChanged (GameStatusChangedArgs args)
    {
        //HideUI
        if(args.CurStatus == GameStatus.Recycle)
        {
            selectMenu.gameObject.SetActive(false);
        }
    }

    public void RefreshSlot(int slotId, ItemData item, int count)
    {
        if(!_inited)
        {
            InitSlot();
        }
        
        if(item == null || count == 0)
        {
            slotList[slotId].SetEmpty();
        }
        else
        {
            slotList[slotId].SetItem(item, count);
        }
    }

    private void OnEquipChanged (int equipId, int slotId) {
        slotList[_currentSelect].SetSelect(false);
        slotList[slotId].SetSelect(true);
        _currentSelect = slotId;
    }

    public void TryShowMenu(int slotId)
    {
        if(InventoryManager.Instance.IsSlotEmpty(slotId))
        {
            return;
        }
        _menuSelect= slotId;
        selectMenu.gameObject.SetActive(true);
        selectMenu.position = slotList[slotId].transform.position;
        craftlistButton.interactable = CraftManager.Instance.HasCraftRecipes(InventoryManager.Instance.GetSlotItemID(slotId));
    }

    public void CraftMenuButtonPressed()
    {
        if(InventoryManager.Instance.TryShowCraftMenu(_menuSelect))
        {
            selectMenu.gameObject.SetActive(false);
        }
    }

    public void DeleteButtonPressed()
    {
        InventoryManager.Instance.DeleteSlot(_menuSelect);
        selectMenu.gameObject.SetActive(false);
    }

    public void ToggleCraftMenu()
    {
        CraftManager.Instance.ToggleCraftMenu();
    }
}
