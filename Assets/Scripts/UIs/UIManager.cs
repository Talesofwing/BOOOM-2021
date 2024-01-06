using System;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;
using Object = UnityEngine.Object;

public enum UIType {
    Menu,
    Inventory,
    BlackCanvasUI,
    CraftMenu,
    SelectedItemUI,
    TipUI,
    SoundSetting,    
    ESCSetting,
    DialogUI,
    ProducerUI
}

public static class UIManager {
    private static BaseUI[] m_UIPrefabs;
    private static List<BaseUI> m_OpenningUIs = new List<BaseUI> (); // openning uis.
    private static List<BaseUI> m_ClosingUIs = new List<BaseUI> (); // closing uis.

    public static void LoadAllUIs () {
        m_UIPrefabs = Resources.LoadAll<BaseUI> (GameConstant.Path.c_RESOURCE_UI_PATH);
    }

    // Create a new ui to pool.
    private static BaseUI CreateUI (UIType type) {
        BaseUI ui = GetUIPrefab (type);
        if (ui) {
            ui = Object.Instantiate (ui);
        } else {
            Debug.Log ("[UIManager CreateUI] : The " + type + " ui isn't exist.");
        }

        return ui;
    }

    private static BaseUI[] CreateUIs (UIType[] types) {
        List<BaseUI> uis = new List<BaseUI> ();
        for (int i = 0; i < types.Length; i++) {
            BaseUI ui = CreateUI (types[i]);
            uis.Add (ui);
        }

        return uis.ToArray ();
    }

    // Open the ui.
    public static BaseUI OpenUI (UIType type, params object[] args) {
        // find the ui in the pool.
        BaseUI ui = GetClosingUI (type);
        if (!ui) {
            // if hasn't ui in the pool, create a new ui.
            ui = CreateUI (type);
        } else {
            m_ClosingUIs.Remove (ui);
        }
        ui.Load (args);
        ui.Open ();

        m_OpenningUIs.Add (ui);

        switch (type) {
            case UIType.Inventory:
                break;
            case UIType.SoundSetting:
                AnalyticsManager.Track(AnalyticsEventKey.VisitScreen,
                    AnalyticsManager.GetDictionary("screen", type.ToString()));
                break;
            case UIType.ESCSetting:
                AnalyticsManager.Track(AnalyticsEventKey.VisitScreen,
                    AnalyticsManager.GetDictionary("screen", type.ToString()));
                break;
            case UIType.CraftMenu:
                break;
            case UIType.Menu:
                break;
            case UIType.BlackCanvasUI:
                break;
            case UIType.SelectedItemUI:
                break;
            case UIType.TipUI:
                break;
            case UIType.DialogUI:
                break;
            case UIType.ProducerUI:
                AnalyticsManager.Track(AnalyticsEventKey.VisitScreen,
                    AnalyticsManager.GetDictionary("screen", type.ToString()));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        return ui;
    }

    // Open the uis
    public static void OpenUIs (UIType[] types) {
        for (int i = 0; i < types.Length; i++) {
            OpenUI (types[i]);
        }
    }

    public static void OpenUIAndCloseUI (UIType openType, BaseUI closeUI) {
        CloseUI (closeUI);
        OpenUI (openType);
    }

    // Close ui, if not to pool, destroy it.
    public static void CloseUI (BaseUI ui, bool toPool = true) {
        // find ui in openning list
        if (m_OpenningUIs.Contains (ui)) {
            m_OpenningUIs.Remove (ui);

            if (toPool) {
                ui.Close ();
                m_ClosingUIs.Add (ui);
            } else {
                ui.Destroy ();
            }
        }
    }

    public static void CloseUI (UIType type, bool toPool = true) {
        // find ui in openning list
        BaseUI ui = GetOpenningUI (type);
        CloseUI(ui, toPool);
    }
    
    // Close uis, if not to pool, destroy it.
    public static void CloseUIs (BaseUI[] uis, bool toPool = true) {
        for (int i = 0; i < uis.Length; i++) {
            CloseUI (uis[i], toPool);
        }
    }

    public static void CloseUIs (UIType type, bool toPool = true) {
        BaseUI[] uis = GetOpenningUIs (type);
        for (int i = 0; i < uis.Length; i++) {
            CloseUI (uis[i], toPool);
        }
    }

    public static bool IsOpenning (UIType type) {
        BaseUI ui = GetOpenningUI (type);

        return ui != null;
    }

#region Helper

    private static BaseUI GetUIPrefab (UIType type) {
        for (int i = 0; i < m_UIPrefabs.Length; i++) {
            if (m_UIPrefabs[i].GetUIType () == type) {
                return m_UIPrefabs[i];
            }
        }

        return null;
    }

    private static BaseUI GetClosingUI (UIType type) {
        for (int i = 0; i < m_ClosingUIs.Count; i++) {
            if (m_ClosingUIs[i].GetUIType () == type) {
                return m_ClosingUIs[i];
            }
        }

        return null;
    }

    private static BaseUI[] GetOpenningUIs (UIType type) {
        List<BaseUI> uis = new List<BaseUI> ();

        for (int i = 0; i < m_OpenningUIs.Count; i++) {
            if (m_OpenningUIs[i].GetUIType () == type) {
                uis.Add (m_OpenningUIs[i]);
            }
        }

        return uis.ToArray ();
    }

    private static BaseUI GetOpenningUI (UIType type) {
        for (int i = 0; i < m_OpenningUIs.Count; i++) {
            if (m_OpenningUIs[i].GetUIType () == type) {
                return m_OpenningUIs[i];
            }
        }

        return null;
    }

#endregion

#region Debug

    public static void DisplayClosingUIs () {
        Debug.Log ("ClosingUIs Count: " + m_ClosingUIs.Count);
        for (int i = 0; i < m_ClosingUIs.Count; i++) {
            Debug.Log (m_ClosingUIs[i].GetUIType ());
        }
    }
    
#endregion

}