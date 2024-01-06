using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceTilemapFeedback : BaseInteractionFeedback {
    [SerializeField] private TilemapType m_UnlockTilemapType;
    [SerializeField] private int m_Cost = 1;
    [SerializeField] private int m_ReplaceCount = 3;

    public override void Excute () {
        int id = InventoryManager.Instance.CurrentEquip;
        if (InventoryManager.Instance.TryCostItem (id, m_Cost)) {
            AnalyticsManager.Track(AnalyticsEventKey.Interact, AnalyticsManager.GetDictionary("type", "Put_Item"));
            
            for (int i = 0; i < m_ReplaceCount; i++) {
                Vector3Int cellPos = MapManager.Instance.GetTilePosInFrontOfPlayer (i + 1);
                MapManager.Instance.RemoveTilemap (m_UnlockTilemapType, cellPos);
                MapManager.Instance.UnlockTilemap (id, m_UnlockTilemapType, cellPos);
                MapManager.Instance.ReplaceEmptyTilemap (GameTool.getTileOfItem (id), cellPos);
                
                Dictionary<string, object> dict = new Dictionary<string, object> ();
                dict["position"] = MapManager.Instance.GetWorldPosByCellPos (cellPos);
                AnalyticsManager.Track(AnalyticsEventKey.PutItems, dict);
            }
        }
    }

}
