using UnityEngine.UI;

public class SoundSettingUI : BaseUI {
    
    public override UIType GetUIType () {
        return UIType.SoundSetting;
    }

    public Slider bgm;
    public Slider se;

    void Start()
    {
        bgm.value = AudioManager.Instance.BGMVol;
        se.value = AudioManager.Instance.SEVol;

        bgm.onValueChanged.AddListener(OnBGMVolumeChange);
        se.onValueChanged.AddListener(OnSEVolumeChange);
    }

    public void OnBGMVolumeChange(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }
    
    public void OnSEVolumeChange(float value)
    {
        AudioManager.Instance.SetSEVolume(value);
    }

    public void CloseButton()
    {
        Close();
    }
}