using System.Collections.Generic;
using UnityEngine;

public class UnlockEntityFeedback : BaseInteractionFeedback {
    [SerializeField] private ScreenEntity m_Entity;
    
    public override void Excute () {
        InventoryManager.Instance.TryCostItem (InventoryManager.Instance.CurrentEquip, 1);
        
        AnalyticsManager.Track(AnalyticsEventKey.Interact, AnalyticsManager.GetDictionary("type", "Put_Item"));

        Dictionary<string, object> dict = new Dictionary<string, object> ();
        dict["position"] = m_Entity.CacheTf.position;
        AnalyticsManager.Track(AnalyticsEventKey.PutItems, dict);
        
        MapManager.Instance.SetupPreservableEntityStatus (m_Entity, true);
    }
    
}
