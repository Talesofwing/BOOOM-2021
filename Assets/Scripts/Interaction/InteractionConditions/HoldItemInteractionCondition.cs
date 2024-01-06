using UnityEngine;

public class HoldItemInteractionCondition : BaseInteractionCondition {
    [SerializeField] private ItemInf[] m_HoldItems;
    
    public override bool CheckInteractable () {
        for (int i = 0; i < m_HoldItems.Length; ++i) {
            if (InventoryManager.Instance.GetItemCount (m_HoldItems[i].ItemId) < m_HoldItems[i].Count) {
                return false;
            }
        }

        return true;
    }

}
