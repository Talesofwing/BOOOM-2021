using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : BaseUI {
    [Header ("組件")] [SerializeField] private Image m_CGImage;
    [SerializeField] private TMP_Text m_DialogText;
    [SerializeField] private GameObject m_CGImageGo;    
    [SerializeField] private GameObject m_AnyClickGo;
    
    [Header ("動畫組件")] [SerializeField] private CanvasGroup m_BlackCanvasGroup;
    [SerializeField] private Animation m_CGImageAnimation;
    [SerializeField] private AnimationDetector m_CGImageAnimationDetector;
    [SerializeField] private CanvasGroup m_TextBoxGroup;

    [Header ("動畫屬性")] [SerializeField] private float m_FadeInOutBlackCanvasDuration = 0.5f;
    [SerializeField] private float m_FadeInOutTextBoxDuration = 0.25f;

    private Scenario m_CurData;
    private int m_DialogIndex = 0;
    private bool m_CanNextDialog = false;
    private bool m_IsBlackBackground;

    public event Action FadeOutStart;
    public event Action Exit;

    private bool m_IsShowing = false;
    
    public override void Load (params object[] args) {
        base.Load (args);

        m_CGImageAnimationDetector.AnimationFinished += OnShowCGImageAnimationFinished;

        m_CurData = (Scenario)args[0];
        m_IsBlackBackground = (bool)args[1];

        AnalyticsManager.Track (AnalyticsEventKey.ShowScenario, AnalyticsManager.GetDictionary ("id", m_CurData.ID));
    }

    private void Update () {
        if (m_IsShowing && m_CanNextDialog && Input.GetKeyDown (KeyCode.Space)) {
            NextDialog ();
        }
    }
    
    public override void Open () {
        base.Open ();

        ShowBlackCanvas ();
        m_IsShowing = true;
    }

    public override void Close () {
        base.Close ();
        
        m_IsShowing = false;
    }
    
    public override UIType GetUIType () {
        return UIType.DialogUI;
    }

    private void ShowBlackCanvas () {
        StartCoroutine (FadeInOutBlackCanvas (true));
    }

    private void HideBlackCanvas () {
        StartCoroutine (FadeInOutBlackCanvas (false));
    }

    IEnumerator FadeInOutBlackCanvas (bool isFadeIn) {
        if (!isFadeIn) {
            if (FadeOutStart != null) {
                FadeOutStart ();
                FadeOutStart = null;
            }
        }

        float time = 0.0f;

        float startingAlpha;
        float endingAlpha;

        if (isFadeIn) {
            if (!m_IsBlackBackground) {
                startingAlpha = 0.0f;
                endingAlpha = 0.5f;
            } else {
                startingAlpha = 0.0f;
                endingAlpha = 1.0f;
            }
        } else {
            if (!m_IsBlackBackground) {
                startingAlpha = 0.5f;
                endingAlpha = 0.0f;
            } else {
                startingAlpha = 1.0f;
                endingAlpha = 0.0f;
            }
        }

        m_BlackCanvasGroup.alpha = startingAlpha;

        while (time <= m_FadeInOutBlackCanvasDuration) {
            time += Time.deltaTime;

            m_BlackCanvasGroup.alpha = Mathf.Lerp (startingAlpha, endingAlpha, time / m_FadeInOutBlackCanvasDuration);

            yield return null;
        }

        m_BlackCanvasGroup.alpha = endingAlpha;

        if (isFadeIn) {
            ShowCGImage ();
        } else {
            if (Exit != null) {
                Exit ();
                Exit = null;
            }
            UIManager.CloseUI (this);
        }
    }

    private void ShowCGImage () {
        if (m_CurData.CG) {
            m_CGImageGo.SetActive (true);
            m_CGImage.sprite = m_CurData.CG;
        } else {
            m_CGImageGo.SetActive (false);
            OnShowCGImageAnimationFinished ();
        }
    }

    private void HideCGImage () {
        m_CGImageAnimation.Play ("CG_Image_Hide");
    }

    private void OnShowCGImageAnimationFinished () {
        ShowTextBox ();

        m_CGImageAnimationDetector.AnimationFinished -= OnShowCGImageAnimationFinished;
        m_CGImageAnimationDetector.AnimationFinished += OnHideCGImageAnimationFinished;
    }

    private void OnHideCGImageAnimationFinished () {
        m_CGImageAnimationDetector.AnimationFinished -= OnHideCGImageAnimationFinished;
        m_CGImage.gameObject.SetActive (false);        
        
        if (m_CurData.NextScenario) {
            m_CurData = m_CurData.NextScenario;
            m_CGImageAnimationDetector.AnimationFinished += OnShowCGImageAnimationFinished;
            ShowCGImage ();
        } else {
            HideBlackCanvas ();
        }
    }

    private void ShowTextBox () {
        if (m_CurData.Desc != null && m_CurData.Desc.Count >= 1) {
            m_DialogText.text = m_CurData.Desc[m_DialogIndex];
            StartCoroutine (FadeInOutTextBox (true));
        } else {
            m_CanNextDialog = true;
            m_AnyClickGo.SetActive (true);
        }
    }

    private void HideTextBox (bool inNextDialog = false) {
        if (m_CurData.Desc != null && m_CurData.Desc.Count >= 1) {
            StartCoroutine (FadeInOutTextBox (false, inNextDialog));
        } else {
            if (!inNextDialog) {
                if (m_CGImageGo.activeSelf) {
                    HideCGImage ();
                } else {
                    OnHideCGImageAnimationFinished ();
                }
            } else {
                ShowTextBox ();
            }
        }
        
    }

    IEnumerator FadeInOutTextBox (bool isFadeIn, bool inNextDialog = true) {
        float time = 0.0f;

        float startingAlpha;
        float endingAlpha;

        if (isFadeIn) {
            startingAlpha = 0.0f;
            endingAlpha = 1.0f;
        } else {
            startingAlpha = 1.0f;
            endingAlpha = 0.0f;
        }

        m_TextBoxGroup.alpha = startingAlpha;

        while (time <= m_FadeInOutBlackCanvasDuration) {
            time += Time.deltaTime;

            m_TextBoxGroup.alpha = Mathf.Lerp (startingAlpha, endingAlpha, time / m_FadeInOutTextBoxDuration);

            yield return null;
        }

        m_TextBoxGroup.alpha = endingAlpha;

        if (isFadeIn) {
            m_CanNextDialog = true;
            m_AnyClickGo.SetActive (true);
        } else {
            if (!inNextDialog) {
                if (m_CGImageGo.activeSelf) {
                    HideCGImage ();
                } else {
                    OnHideCGImageAnimationFinished ();
                }
            } else {
                ShowTextBox ();
            }
        }
    }

    private void NextDialog () {
        if (!m_CanNextDialog)
            return;

        m_AnyClickGo.SetActive (false);
        m_DialogIndex++;
        m_CanNextDialog = false;
        
        if (m_DialogIndex >= m_CurData.Desc.Count) {
            // 該節已結束
            m_DialogIndex = 0;
            HideTextBox ();

            return;
        }

        HideTextBox (true);
    }

    public void OnNextDialogButtonClick () {
        NextDialog ();
    }

}
