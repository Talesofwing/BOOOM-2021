using UnityEngine;

public class PlayAudioTimerEvent : BaseTimerEvent {
    [SerializeField] private bool m_IsBgm = false;
    [SerializeField] private bool m_PlayBackground = false;
    [SerializeField] private AudioClip m_AudioClip;
    
    private bool m_Visibility = false;
    
    public override void Excute () {
        if (m_PlayBackground) {
            PlayAudio ();
        } else {
            if (m_Visibility) {
                PlayAudio ();
            }
        }
        
        base.Excute ();
    }

    private void PlayAudio () {
        if (m_IsBgm) {
            AudioManager.Instance.PlayBGM (m_AudioClip);
        } else {
            AudioManager.Instance.PlaySE (m_AudioClip);
        }
    }
    
    private void OnBecameVisible () {
        m_Visibility = true;
    }

    private void OnBecameInvisible () {
        m_Visibility = false;
    }
}
