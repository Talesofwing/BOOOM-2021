using Kuma.Utils.Singleton;
using Kuma.Utils.UpdateManager;
using UnityEngine;

public enum FunctionType {
    ShowCraftUI,
    Interact,
    Save,
}

[System.Serializable]
public struct InputElement {
    public FunctionType Type;
    public KeyCode KeyPress;
}

public class GameInputManager : MonoSingleton<GameInputManager>, IUpdatable {
    [SerializeField] private InputElement[] m_InputElements;
    
    private void Start () {
        UpdateDispatcher.Instance.AddUpdatable (this);
    }

    public void CallUpdate (float dt) {
        if (GameManager.Instance.GameStatus != GameStatus.Gaming) {
            if (GameManager.Instance.GameStatus == GameStatus.Pausing) {
                if (Input.GetKeyDown (KeyCode.Escape)) {
                    UIManager.CloseUI (UIType.ESCSetting);
                }
            }

            return;
        }
        
        for (int i = 0; i < m_InputElements.Length; i++) {
            InputElement element = m_InputElements[i];
            if (Input.GetKeyDown (element.KeyPress)) {
                InvokeFunction (element);
            }
        }

        if (Input.GetKeyDown (KeyCode.Alpha0)) {
            InventoryManager.Instance.SelectEquip (9);
        } else {
            for (int slot = 0; slot < 9; slot++) {
                if (Input.GetKeyDown (KeyCode.Alpha1 + slot)) {
                    InventoryManager.Instance.SelectEquip (slot);
                }
            }
        }

        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (!UIManager.IsOpenning (UIType.CraftMenu)) {
                UIManager.OpenUI (UIType.ESCSetting);
            } else {
                CraftManager.Instance.ToggleCraftMenu ();
            }
        }

    }

    private void OnDestroy () {
        if (UpdateDispatcher.Instance != null) {
            UpdateDispatcher.Instance.RemoveUpdatable(this);
        }
    }

    private void InvokeFunction (InputElement element) {
        switch (element.Type) {
            case FunctionType.ShowCraftUI:
                CraftManager.Instance.ToggleCraftMenu();
                break;
            case FunctionType.Interact:
                MapManager.Instance.CachePlayer.Interact ();
                break;
            case FunctionType.Save:
                SaveManager.SaveGameData ();
                UIManager.OpenUI (UIType.TipUI, "保存成功!");
                break;
        }
    }
}
