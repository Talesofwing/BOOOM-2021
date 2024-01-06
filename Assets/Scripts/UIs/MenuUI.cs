using UnityEngine.UI;

public class MenuUI : BaseUI {
    
    public override UIType GetUIType () {
        return UIType.Menu;
    }

    public Button continueButton;

    void Start()
    {
       continueButton.interactable = SaveManager.SaveLoaded;
    }

    public void StartGameClick () {
        BlackCanvasUI ui = (BlackCanvasUI)UIManager.OpenUI (UIType.BlackCanvasUI);
        ui.FadeInAnimationFinished += () => {
            SceneManager.OpenScene (SceneType.Game, false, true);
            UIManager.CloseUI (this);
        };
        ui.FadeInAndFadeOut ();
    }

    public void ContinueClick () {
        BlackCanvasUI ui = (BlackCanvasUI)UIManager.OpenUI (UIType.BlackCanvasUI);
        ui.FadeInAnimationFinished += () => {
            SceneManager.OpenScene (SceneType.Game, false, false);
            UIManager.CloseUI (this);
        };
        ui.FadeInAndFadeOut ();
    }

    public void ProducerButtonClick () {
        UIManager.OpenUI (UIType.ProducerUI);
    }
    
    public void SettingClick() {
        UIManager.OpenUI(UIType.SoundSetting);
    }

    public void ExitClick() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
    
}