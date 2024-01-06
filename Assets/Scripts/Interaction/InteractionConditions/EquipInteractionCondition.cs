using UnityEngine;

public class EquipInteractionCondition : BaseInteractionCondition {
    [SerializeField] private ItemData m_EquipedItem;
    
    public override bool CheckInteractable () {
        if (InventoryManager.Instance.CurrentEquip == m_EquipedItem.ID) {
            return true;
        } else {
            return false;
        }
    }

}
