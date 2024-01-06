using UnityEngine;
using UnityEngine.UI;

public class EscSettingUI : BaseUI {
    
    public override UIType GetUIType () {
        return UIType.ESCSetting;
    }

    public GameObject saveDone;

    public override void Open () {
        base.Open ();

        GameManager.Instance.GamePause ();
    }

    public override void Close () {
        base.Close ();

        GameManager.Instance.GameContinue ();
    }
    
    void OnEnable()
    {
        saveDone.SetActive(false);
    }

    public void CloseButton()
    {
        Close();
    }

    public void SaveButton()
    {
        Save();
    }

    public void SettingButton()
    {
        UIManager.OpenUI(UIType.SoundSetting);
    }

    public void QuitButton()
    {
        Quit();
    }

    public void HomeButton()
    {
        BlackCanvasUI ui = (BlackCanvasUI)UIManager.OpenUI (UIType.BlackCanvasUI);
        ui.FadeInAnimationFinished += () => {
            GameManager.Instance.GameClose ();
            SceneManager.OpenScene (SceneType.Menu, false, true);
            UIManager.CloseUI (this, false);
        };
        ui.FadeInAndFadeOut ();
    }

    public void SaveAndQuitButton()
    {
        Save();
        Quit();
    }

    private void Save()
    {
        SaveManager.SaveGameData();
        saveDone.SetActive(true);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
}