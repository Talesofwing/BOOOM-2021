using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kuma.Utils.Singleton;

public class CraftManager : MonoSingleton<CraftManager>
{
    private List<ItemData> craftRecipeList;
    private List<bool> recipeUnlocked;
    private List<ItemData> unlockedRecipes;
    private Dictionary<int, List<ItemData>> craftableItemMap;

    private UICraftMenu _craftMenu;

    protected override void Awake() {
        base.Awake ();
        
        InitRecipeMap();
    }

    private void InitRecipeMap()
    {
        var itemList = DataManager.AllItemData;
        craftRecipeList = new List<ItemData>();
        craftableItemMap = new Dictionary<int, List<ItemData>>();
        recipeUnlocked = new List<bool>();

        foreach (var item in itemList)
        {
            var recipe = item.CraftRecipe;
            if(recipe == null || recipe.Count == 0)
            {
                continue;
            }

            bool hasRecipe = false;

            foreach (var material in recipe)
            {
                if(material.cost > 0)
                {
                    var materialId = material.itemId;
                    if(!craftableItemMap.ContainsKey(materialId))
                    {
                        craftableItemMap.Add(materialId, new List<ItemData>());
                    }
                    craftableItemMap[materialId].Add(item);
                    hasRecipe = true;
                }
            }

            if(!hasRecipe)
            {
                continue;
            }/**/

            craftRecipeList.Add(item);
            recipeUnlocked.Add(false);
        }
    }

    public void OnGetItem(int itemId)
    {
        if(!craftableItemMap.ContainsKey(itemId))
        {
            return;
        }
        var recipeList = craftableItemMap[itemId];
        if(recipeList == null || recipeList.Count <= 0)
        {
            return;
        }
        foreach (var recipe in recipeList)
        {
            UnlockRecipe(recipe.ID);
        }
    }

    public void UnlockRecipe(int itemId)
    {
        for (int i = 0; i < craftRecipeList.Count; i++)
        {
            if(craftRecipeList[i].ID == itemId)
            {
                if(!recipeUnlocked[i])
                {
                    recipeUnlocked[i] = true;
                    RefreshUnlockList();
                }
                return;
            }
        }

    }

    private void RefreshUnlockList()
    {  
        unlockedRecipes = new List<ItemData>();
        for (int i = 0; i < craftRecipeList.Count; i++)
        {
            if (recipeUnlocked[i])
            {
                unlockedRecipes.Add(craftRecipeList[i]);
            }
        }
    }

    public List<ItemData> GetRecipeList(bool getUnlocked = true)
    {
        if(getUnlocked)
        {
            return unlockedRecipes;
        }

        return craftRecipeList;
    }

    public bool TryShowCraftMenu(ItemData selectMaterial)
    {
        if(!HasCraftRecipes(selectMaterial.ID))
        {
            return false;
        }

        ShowCraftMenu(craftableItemMap[selectMaterial.ID]);
        return true;
    }

    public bool HasCraftRecipes(int materialID)
    {
        return craftableItemMap.ContainsKey(materialID);
    }

    public void ShowCraftMenu(IEnumerable<ItemData> recipes)
    {
        if(_craftMenu == null)
        {
            _craftMenu = UIManager.OpenUI(UIType.CraftMenu) as UICraftMenu;
        }
        else {
            _craftMenu = UIManager.OpenUI (UIType.CraftMenu) as UICraftMenu;
        }
        _craftMenu.ShowRecipes(recipes);
    }

    public void ToggleCraftMenu() {
        if(_craftMenu != null)
        {
            UIManager.CloseUI (_craftMenu);
            _craftMenu = null;
        }
        else
        {
            ShowCraftMenu(GetRecipeList());
        }
    }

    //==============Crafting
    public bool TryCraftItem(int itemId)
    {
        if(!CanCraftItem(itemId))
        {
            UIManager.OpenUI (UIType.TipUI, "素材不足!");
            return false;
        }

        var item = DataManager.GetItemData(itemId);

        if(item.CraftRecipe == null)
        {
            Debug.Log("No Recipe");
            return false;
        }
        
        if(!InventoryManager.Instance.CanTakeItem(item.ID, item.CraftCount))
        {
            UIManager.OpenUI (UIType.TipUI, "空間不足!");
            return false;
        }

        foreach (var material in item.CraftRecipe)
        {
            if (!InventoryManager.Instance.TryCostItem(material.itemId, material.cost))
            {
                Debug.Log("Try Cost Material Error");
                return false;
            }
        }

        AudioManager.Instance.PlaySE("craft");

        InventoryManager.Instance.TryTakeItem(item.ID, item.CraftCount);
        _craftMenu.Refresh();

        // 炸彈
        if (item.ID == 25) {
            int id = 105;
            if (!ScenarioManager.Instance.CheckHasBeenShowById (id)) {
                Scenario scenario = ScenarioManager.Instance.GetScenarioDataById (id);
                ScenarioManager.Instance.AddHasBeenShowScenarioId (id);
                GameManager.Instance.GamePause ();
                DialogUI ui = UIManager.OpenUI (UIType.DialogUI, scenario, false) as DialogUI;
                ui.Exit += () => {
                    GameManager.Instance.GameContinue ();
                };
            }
        }
        
        Dictionary<string, object> dict = new Dictionary<string, object> ();
        dict["id"] = itemId;
        dict["num"] = 1;
        AnalyticsManager.Track(AnalyticsEventKey.CraftItems, dict);

        return true;
    }

    public bool CanCraftItem(int itemId)
    {
        var item = DataManager.GetItemData(itemId);

        if(item.CraftRecipe == null)
        {
            Debug.Log("No Recipe");
            return false;
        }

        foreach (var material in item.CraftRecipe)
        {
            var itemCount = InventoryManager.Instance.GetItemCount(material.itemId);
            if (itemCount < material.cost)
            {
                return false;
            }
        }
        

        return true;
    }

}
