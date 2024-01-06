using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
struct TakeItemInf {
    public ItemData Item;
    public Vector2 CountRange;
}

public class DropItemFeedback : BaseInteractionFeedback {
    [SerializeField] private List<TakeItemInf> m_DropItemDatas;
    
    public override bool CheckExecutable () {
        base.CheckExecutable ();

        bool success = true;
        foreach (var inf in m_DropItemDatas) {
            int count = Random.Range ((int)inf.CountRange.x, (int)inf.CountRange.y);
            if (!InventoryManager.Instance.TryTakeItem (inf.Item.ID,  count, true)) {
                success = false;
            }
        }

        return success;
    }

    public override void Excute () {
        AnalyticsManager.Track(AnalyticsEventKey.Interact, AnalyticsManager.GetDictionary("type", "Get_Item"));
        
        foreach (var inf in m_DropItemDatas) {
            int count = Random.Range ((int)inf.CountRange.x, (int)inf.CountRange.y);
            InventoryManager.Instance.TryTakeItem (inf.Item.ID, count);
            
            Dictionary<string, object> dict = new Dictionary<string, object> ();
            dict["id"] = inf.Item.ID;
            dict["num"] = count;
            AnalyticsManager.Track(AnalyticsEventKey.GetItems, dict);
        }
    }
    
}
