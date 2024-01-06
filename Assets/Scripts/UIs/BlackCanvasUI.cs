using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackCanvasUI : BaseUI {
    [SerializeField] private Image m_Image;

    [Header ("動畫屬性")] 
    [SerializeField] private float m_Duraction;
    [SerializeField] private Color m_FadeOutColor = Color.black;
    [SerializeField] private Color m_FadeInColor = Color.black;
    
    public Action FadeOutAnimationFinished;
    public Action FadeInAnimationFinished;

    public override UIType GetUIType () {
        return UIType.BlackCanvasUI;
    }

    public void FadeIn () {
        m_Image.color = m_FadeOutColor;
        
        StartCoroutine (FadeInAnimation ());
    }

    public void FadeOut (bool autoClose = true) {
        m_Image.color = m_FadeInColor;
        
        StartCoroutine (FadeOutAnimation (autoClose));
    }

    public void FadeInAndFadeOut (bool autoClose = true) {
        FadeInAnimationFinished += () => {
            FadeOut(autoClose);
        };
        FadeIn ();
    }
    
    private IEnumerator FadeInAnimation () {
        float time = 0.0f;
        while (time < m_Duraction) {
            time += Time.deltaTime;

            Color color = Color.Lerp (m_FadeOutColor, m_FadeInColor, time);
            m_Image.color = color;
            
            yield return null;
        }

        if (FadeInAnimationFinished != null)
            FadeInAnimationFinished ();

        FadeInAnimationFinished = null;
    } 
    
    private IEnumerator FadeOutAnimation (bool autoClose) {
        float time = 0.0f;
        while (time < m_Duraction) {
            time += Time.deltaTime;

            Color color = Color.Lerp (m_FadeInColor, m_FadeOutColor, time);
            m_Image.color = color;
            
            yield return null;
        }
        
        if (FadeOutAnimationFinished != null)
            FadeOutAnimationFinished ();
        
        FadeOutAnimationFinished = null;

        if (autoClose) {
            UIManager.CloseUI (this);
        }
    }
    
}
