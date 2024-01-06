using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipUI : BaseUI {
    [SerializeField] private GameObject m_TipGo;
    [SerializeField] private GameObject m_RecyelTipGo;
    
    [SerializeField] private Text m_Text;
    [SerializeField] private float m_AutoHideDuration = 3.0f;
    [SerializeField] private RectTransform m_RecycleTipTextTf;
    [SerializeField] private float m_Height = 50.0f;
    
    private Vector3 m_RecycleTipTextStartingPosition;

    private bool m_IsShowRecycleText = false;
    
    public override UIType GetUIType () {
        return UIType.TipUI;
    }

    public override void Load (params object[] args) {
        base.Load();

        if (args.Length > 0) {
            m_Text.text = (string)args[0];
            m_IsShowRecycleText = false;
        } else {
            m_IsShowRecycleText = true;
        }

        m_RecycleTipTextStartingPosition = m_RecycleTipTextTf.position;
        
        GameManager.Instance.GameStatusChangedEvent += OnGameStatusChanged;
    }

    public override void Open () {
        base.Open ();

        if (m_IsShowRecycleText) {
            m_TipGo.SetActive (false);
            m_RecyelTipGo.SetActive (true);
            
            ShowRecycleTipText ();
        } else {
            m_TipGo.SetActive (true);
            m_RecyelTipGo.SetActive (false);
        }
        
        StartCoroutine ("DelayClose");
    }

    public override void Close () {
        base.Close ();

        m_RecycleTipTextTf.position = m_RecycleTipTextStartingPosition;
        
        GameManager.Instance.GameStatusChangedEvent -= OnGameStatusChanged;
    }

    private void AutoClose () {
        UIManager.CloseUI (this);
    }
    
    private void OnGameStatusChanged (GameStatusChangedArgs args) {
        if (args.CurStatus == GameStatus.Recycle || args.CurStatus == GameStatus.Closing) {
            StopCoroutine ("DelayClose");
            StopCoroutine ("RecycleTipTextAnimation");
            AutoClose ();
        }
    }

    private IEnumerator DelayClose () {
        float time = 0.0f;

        while (time <= m_AutoHideDuration) {
            time += Time.deltaTime;

            yield return null;
        }

        AutoClose ();
    }

    private void ShowRecycleTipText () {
        StartCoroutine ("RecycleTipTextAnimation");
    }

    private IEnumerator RecycleTipTextAnimation () {
        float duration = 0.1f;
        float time = 0.0f;

        // 顯示
        Vector3 startingPos = m_RecycleTipTextStartingPosition;
        startingPos.y += m_Height;
        m_RecycleTipTextTf.position = startingPos;
        
        while (time < duration) {
            yield return null;

            Vector3 pos = Vector3.Lerp (startingPos, m_RecycleTipTextStartingPosition, time / duration);
            m_RecycleTipTextTf.position = pos;
            
            time += Time.deltaTime;
        }
        
        m_RecycleTipTextTf.position = m_RecycleTipTextStartingPosition;

        // 暫停
        duration = 1.5f;
        time = 0.0f;
        while (time < duration) {
            yield return null;
            time += Time.deltaTime;
        }
        
        // 隱藏
        duration = 0.1f;
        time = 0.0f;
        while (time < duration) {
            yield return null;

            Vector3 pos = Vector3.Lerp (m_RecycleTipTextStartingPosition, startingPos, time / duration);
            m_RecycleTipTextTf.position = pos;
            
            time += Time.deltaTime;
        }

        m_RecycleTipTextTf.position = startingPos;
    }
    
}
