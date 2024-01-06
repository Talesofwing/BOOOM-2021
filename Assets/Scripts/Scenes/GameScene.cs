using UnityEngine;

public class GameScene : BaseScene {
    private bool m_IsNewGame = false;

    public override SceneType GetSceneType () {
        return SceneType.Game;
    }

    public override void Load (params object[] args) {
        base.Load (args);

        SaveManager.LoadGameData ();
        
        m_IsNewGame = (bool)args[0];

        InventoryManager.Instance.SetInventoryUI (UIManager.OpenUI (UIType.Inventory) as UIInventoryHud);

        GameManager.Instance.GameInit ();
        GameManager.Instance.GameLoad (m_IsNewGame);
    }

    public override void Open () {
        base.Open ();

        AudioManager.Instance.PlayBGM ("Game");
        UIManager.OpenUI (UIType.SelectedItemUI);
        
        if (m_IsNewGame) {
            this.CacheGo.SetActive (false);
            DialogUI ui = (DialogUI)UIManager.OpenUI (UIType.DialogUI, ScenarioManager.Instance.GetScenarioDataById (1), true);
            ui.FadeOutStart += () => {
                this.CacheGo.SetActive (true);
            };
            ui.Exit += () => {
                GameManager.Instance.GameStart ();
            };
        } else {
            GameManager.Instance.GameStart ();
        }
    }

}
