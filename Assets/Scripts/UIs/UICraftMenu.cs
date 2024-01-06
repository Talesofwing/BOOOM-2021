using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICraftMenu : BaseUI
{
    public override UIType GetUIType () {
        return UIType.CraftMenu;
    }

    [SerializeField]private UICraftSheet craftSheetPrefab;
    [SerializeField]private Transform recipeListParent;

    private List<UICraftSheet> sheetList = new List<UICraftSheet>();

    void OnEnable()
    {
        GameManager.Instance.GameStatusChangedEvent += OnGameStatusChanged;
    }

    public void ShowRecipes(IEnumerable<ItemData> recipes)
    {
        var counter = 0;
        if(recipes != null && recipes.Count() > 0)
        {
            foreach (var item in recipes)
            {
                UICraftSheet recipeSheet;
                if(counter < sheetList.Count)
                {
                    recipeSheet = sheetList[counter];
                    recipeSheet.gameObject.SetActive(true);
                }
                else
                {
                    recipeSheet = Instantiate(craftSheetPrefab, recipeListParent);
                    sheetList.Add(recipeSheet);
                }
                recipeSheet.Init(item);
                counter++;
            }
        }

        while (counter < sheetList.Count)
        {
            sheetList[counter].gameObject.SetActive(false);
            counter++;
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < sheetList.Count; i++)
        {
            if(sheetList[i].gameObject.activeInHierarchy)
            {
                sheetList[i].Refresh();
            }
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GameStatusChangedEvent -= OnGameStatusChanged;
    }

    public void CloseButtonClick () {
        CraftManager.Instance.ToggleCraftMenu ();
    }
    
    public void OnGameStatusChanged (GameStatusChangedArgs args)
    {
        // //HideUI
        // if(args.CurStatus == GameStatus.Timeout)
        // {
        //     Close();
        // }
    }
}
