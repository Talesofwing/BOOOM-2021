using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Kuma.Utils.UpdateManager;

public class UIInventoryItemSlot : BaseMono, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI keyCode;
    [SerializeField] private GameObject selectFrame;
    [SerializeField] private GameObject garyFrame;

    public int SlotId{get; private set;}
    private UIInventoryHud _owner;

    public bool isCraftMaterial;

    private int _itemId;
    private int _itemCount;

    public void Init(UIInventoryHud owner, int id)
    {
        _owner = owner;
        SlotId = id;
        selectFrame.SetActive(false);
        SetEmpty();
    }

    public void SetKeycode(string key)
    {
        if(key.Length > 0)
        {
            keyCode.text = "[" + key + "]";
        }
        else
        {
            keyCode.text = "";
        }
    }

    public void SetEmpty()
    {
        itemIcon.gameObject.SetActive(false);
        itemIcon.sprite = null;
        itemCount.text = "";
        itemName.text = "";
    }

    public void SetItem(ItemData item, int count)
    {
        _itemId = item.ID;
        _itemCount = count;

        itemIcon.sprite = item.Icon;
        if(!itemIcon.gameObject.activeSelf)
        {
            itemIcon.gameObject.SetActive(true);
        }

        itemName.text = item.Name;

        if(count > 1)
        {
            itemCount.text = "x" + count;
        }
        else
        {
            itemCount.text = "";
        }

        Refresh();
    }

    public void SetSelect(bool select)
    {
        if(selectFrame.activeSelf != select)
        {
            selectFrame.SetActive(select);
        }
    }

    public void Refresh()
    {
        if(isCraftMaterial)
        {
            garyFrame.SetActive(InventoryManager.Instance.GetItemCount(_itemId) < _itemCount);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_owner == null)
        {
            return;
        }

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryManager.Instance.SelectEquip(this.SlotId);
        }

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            _owner.TryShowMenu(this.SlotId);
        }
    }
}
