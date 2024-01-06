using Kuma.Utils.Singleton;
using UnityEngine;



public class CheatingMachine : MonoSingleton<CheatingMachine> {
    [SerializeField] private int m_AddCount = 5;
    [SerializeField] private KeyCode m_AddAllPressKey;
    [SerializeField] private CheatingElement[] m_Elements;

    public void Update () {
        for (int i = 0; i < m_Elements.Length; i++) {
            CheatingElement element = m_Elements[i];
            if (Input.GetKeyDown (element.PressKey)) {
                for (int j = 0; j < element.Items.Length; ++j) {
                    int id = element.Items[j].ID;
                    InventoryManager.Instance.TryTakeItem (id, m_AddCount);
                }
            }
        }

        if (Input.GetKeyDown (m_AddAllPressKey)) {
            var itemList = DataManager.AllItemData;

            foreach (var item in itemList) {
                InventoryManager.Instance.TryTakeItem(item.ID, Mathf.Min(10, item.MaxStack));
            }
        }

        //debug SaveLoad

        if (Input.GetKeyDown (KeyCode.L)) 
        {
            SaveManager.LoadGameData();
            InventoryManager.Instance.LoadSaveData(SaveManager.GetInventorySave());
        }
        else if (Input.GetKeyDown (KeyCode.K)) 
        {
            SaveManager.SaveGameData();
        }
    }

    [System.Serializable]
    private struct CheatingElement {
        public KeyCode PressKey;
        public ItemData[] Items;
    }

}
