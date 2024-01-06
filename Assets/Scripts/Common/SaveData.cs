using UnityEngine;

public struct MapData {
    public Vector3 PlayerPos;
    public TileData[] UnlockedRiverTilemapDatas;               // River tilemap
    public TileData[] UnlockedCorrosiveRiverTilemapDatas;      // Corrosive River tilemap
    public bool[] TilemapTriggersAliveStatus;                  // Trigger的存活狀態 
    public ScreenEntityData[] PreservableScreenEntityDatas;    // 場景上的可保存物體數據
    
    [System.Serializable]
    public struct TileData {
        public int ItemId;
        public Vector3Int TilePos;        // Tilemap的座標
    }

    [System.Serializable]
    public struct ScreenEntityData {
        public bool Activity;
    }
}

public struct ScenarioData {
    public int[] HasBeenShowScenarioIds;
}

public struct InventoryData
{
    public int[] itemIds;
    public int[] itemCounts;
}