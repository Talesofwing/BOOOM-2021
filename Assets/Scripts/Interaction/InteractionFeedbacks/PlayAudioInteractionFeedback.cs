using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioInteractionFeedback : BaseInteractionFeedback {
    [SerializeField] private AudioClip m_Sound;

    public override void Excute () {
        if (m_Sound == null) {
            AudioManager.Instance.PlayInteraction ();
        } else {
            AudioManager.Instance.PlaySE (m_Sound);
        }
    }

}
