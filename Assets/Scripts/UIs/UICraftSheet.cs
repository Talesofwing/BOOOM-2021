using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSheet : BaseMono, IPointerClickHandler
{
    [SerializeField]private TextMeshProUGUI itemName;
    [SerializeField]private UIInventoryItemSlot recipeItem;
    [SerializeField]private GameObject gray;
    [SerializeField]private UIInventoryItemSlot[] materials;

    private ItemData _targetItem;

    public void Init(ItemData recipe)
    {
        _targetItem = recipe;
        itemName.text = recipe.Name;
        recipeItem.SetItem(recipe, recipe.CraftCount);

        for (int i = 0; i < materials.Length; i++)
        {
            if(i < recipe.CraftRecipe.Count)
            {
                materials[i].gameObject.SetActive(true);
                materials[i].SetItem(recipe.CraftRecipe[i].item, recipe.CraftRecipe[i].cost);
            }
            else
            {
                materials[i].gameObject.SetActive(false);
            }
        }
        gray.SetActive(!CraftManager.Instance.CanCraftItem(_targetItem.ID));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftManager.Instance.TryCraftItem(_targetItem.ID);
    }

    public void Refresh()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if(materials[i].gameObject.activeInHierarchy)
            {
                materials[i].Refresh();
            }
        }
        gray.SetActive(!CraftManager.Instance.CanCraftItem(_targetItem.ID));
    }
}
