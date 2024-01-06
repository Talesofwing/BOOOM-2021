using System.Collections.Generic;
using Kuma;
using Kuma.Utils.Singleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public enum TilemapType {
    River,
    CorrosiveRiverTilemap
}

public class MapManager : MonoSingleton<MapManager> {
    [Header ("玩家")] [SerializeField] private Player m_Player;

    [Header ("觸發器")] [SerializeField] private Transform m_TriggersRoot;

    [Header ("Tilemap")] [SerializeField] private Grid m_MapGrid;
    [SerializeField] private Tilemap m_RiverTilemap;
    [SerializeField] private Tilemap m_CorrosiveRiverTilemap;
    [SerializeField] private Tilemap m_EmptyTilemap;

    [Header ("可保存的場景物品")] [SerializeField] private Transform m_PreservableEntityRoot;

    // TilemapTrigger
    private TilemapTrigger[] m_TilemapTriggers;
    private bool[] m_TilemapTriggersAliveStatus;

    // Unlock bridge position
    private List<MapData.TileData> m_RiverTilemapDatas = new List<MapData.TileData> ();
    private List<MapData.TileData> m_CorrosiveRiverTilemapDatas = new List<MapData.TileData> ();

    // Preservable screenobject
    private Entity[] m_PreservableEntities;
    private MapData.ScreenEntityData[] m_PreservableEntityDatas;

    public Player CachePlayer => m_Player;

    private List<ScreenEntity> m_Entities = new List<ScreenEntity> ();

    public void Load (bool isNewGame) {
        if (!isNewGame) {
            // Get save data.
            MapData saveData = SaveManager.GetMapSave ();

            // Player pos
            CachePlayer.CacheTf.position = saveData.PlayerPos;

            // Tilemap's data

            // River
            MapData.TileData[] riverDatas = saveData.UnlockedRiverTilemapDatas;
            foreach (var data in riverDatas) {
                m_RiverTilemapDatas.Add (data);

                ReplaceEmptyTilemap (GameTool.getTileOfItem (data.ItemId), data.TilePos);
                RemoveTilemap (TilemapType.River, data.TilePos);
            }

            // Corrosive River
            MapData.TileData[] corrosiveRiverDatas = saveData.UnlockedCorrosiveRiverTilemapDatas;
            foreach (var data in corrosiveRiverDatas) {
                m_CorrosiveRiverTilemapDatas.Add (data);

                ReplaceEmptyTilemap (GameTool.getTileOfItem (data.ItemId), data.TilePos);
                RemoveTilemap (TilemapType.CorrosiveRiverTilemap, data.TilePos);
            }

            // TilemapTrigger's data
            m_TilemapTriggersAliveStatus = saveData.TilemapTriggersAliveStatus;

            // Load Preservable Screen Entity data and setup.
            m_PreservableEntityDatas = saveData.PreservableScreenEntityDatas;
            for (int i = 0; i < m_PreservableEntityDatas.Length; i++) {
                if (!m_PreservableEntityDatas[i].Activity)
                    m_PreservableEntities[i].Remove ();
                else {
                    m_PreservableEntities[i].CacheGo.SetActive (true);
                }
            }

        } else {
            // 初始化tilemap trigger datas
            m_TilemapTriggersAliveStatus = new bool[m_TilemapTriggers.Length];
            for (int i = 0; i < m_TilemapTriggersAliveStatus.Length; i++) {
                m_TilemapTriggersAliveStatus[i] = true;
            }

            // 初始化preservable screen entity datas.
            m_PreservableEntityDatas = new MapData.ScreenEntityData[m_PreservableEntities.Length];
            for (int i = 0; i < m_PreservableEntityDatas.Length; i++) {
                m_PreservableEntityDatas[i].Activity = m_PreservableEntities[i].CacheGo.activeSelf;
            }
        }

        foreach (var entity in m_Entities) {
            entity.Load ();
        }

        m_Player.Load ();

        // Load TilemapTrigger's data and setup.
        for (int i = 0; i < m_TilemapTriggers.Length; i++) {
            m_TilemapTriggers[i].Setup (i, m_TilemapTriggersAliveStatus[i]);
        }

    }

    public void Init () {
        CameraController.Instance.SetTarget (m_Player.CacheTf);

        foreach (var entity in m_Entities) {
            entity.Init ();
        }

        m_Player.Init ();

        // Load TilemapTrigger's component.
        m_TilemapTriggers = m_TriggersRoot.GetComponentsInChildren<TilemapTrigger> ();

        // Load preservable entity
        m_PreservableEntities = m_PreservableEntityRoot.GetComponentsInChildren<Entity> (true);
    }

    public void Recycle () {
        foreach (var entity in m_Entities) {
            entity.Recycle ();
        }

        m_Player.Recycle ();

        for (int i = 0; i < m_PreservableEntityDatas.Length; i++) {
            bool activity = m_PreservableEntityDatas[i].Activity;
            m_PreservableEntities[i].CacheGo.SetActive (activity);
        }
    }

    public void Timeout () {
        foreach (var entity in m_Entities) {
            entity.Timeout ();
        }

        m_Player.Timeout ();
    }

    public void GameClose () {
        m_Player.GameClose ();
    }

    public void RegisterEntity (ScreenEntity entity) {
        m_Entities.Add (entity);
    }

    public void UnregisterEntity (ScreenEntity entity) {
        m_Entities.Remove (entity);
    }

    // offset是指主角前方的第X格
    // 0 代表身下的一格
    public Vector2 GetPosInFrontOfPlayer (int offset) {
        Vector2 pos = m_Player.CacheTf.position;
        switch (m_Player.FaceTo) {
            case Direction.Right:
                pos += Vector2.right * offset;

                break;
            case Direction.Left:
                pos += Vector2.left * offset;

                break;
            case Direction.Upper:
                pos += Vector2.up * offset;

                break;
            case Direction.Lower:
                pos += Vector2.down * offset;

                break;
        }

        return pos;
    }

    public void SetupPreservableEntityStatus (Entity entity, bool activity) {
        int index = FindScreenEntityIndex (entity);
        m_PreservableEntityDatas[index].Activity = activity;
        if (!activity) {
            m_PreservableEntities[index].Remove ();
        } else {
            m_PreservableEntities[index].CacheGo.SetActive (true);
        }
    }

    private int FindScreenEntityIndex (Entity entity) {
        for (int i = 0; i < m_PreservableEntities.Length; i++) {
            if (entity == m_PreservableEntities[i]) {
                return i;
            }
        }

        return -1;
    }

    public void SetupTilemapTrigger (int id) {
        m_TilemapTriggersAliveStatus[id] = false;
    }

#region Tilemap

    public Vector2 GetWorldPosByTileInFrontOfPlayer () {
        Vector3Int cellPos = GetTilePosInFrontOfPlayer (1);
        Vector3 worldPos = m_MapGrid.CellToWorld (cellPos);

        return worldPos;
    }

    public Vector3 GetWorldPosByCellPos (Vector3Int cellPos) {
        return m_MapGrid.CellToWorld (cellPos);
    }
    
    // offset是指主角前方的第X格
    // 0 代表身下的一格
    public Vector3Int GetTilePosInFrontOfPlayer (int offset) {
        Vector2 pos = GetPosInFrontOfPlayer (offset);

        return m_MapGrid.WorldToCell (pos);
    }

    // 這裏的tilePos是指Tilemap的座標
    public void RemoveTilemap (TilemapType type, Vector3Int tilePos) {
        switch (type) {
            case TilemapType.River:
                m_RiverTilemap.SetTile (tilePos, null);

                break;
            case TilemapType.CorrosiveRiverTilemap:
                m_CorrosiveRiverTilemap.SetTile (tilePos, null);

                break;
        }
    }

    // 這裏的tilePos是指Tilemap的座標
    public void ReplaceEmptyTilemap (TileBase tile, Vector3Int tilePos) {
        m_EmptyTilemap.SetTile (tilePos, tile);
    }

    public void UnlockTilemap (int id, TilemapType type, Vector3Int tilePos) {
        switch (type) {
            case TilemapType.River:
                m_RiverTilemapDatas.Add (new MapData.TileData () {
                    ItemId = id,
                    TilePos = tilePos
                });

                break;
            case TilemapType.CorrosiveRiverTilemap:
                m_CorrosiveRiverTilemapDatas.Add (new MapData.TileData () {
                    ItemId = id,
                    TilePos = tilePos
                });

                break;
        }
    }

#endregion

    public MapData GetSaveData () {
        MapData data;
        data.PlayerPos = m_Player.CacheTf.position;
        data.UnlockedRiverTilemapDatas = m_RiverTilemapDatas.ToArray ();
        data.UnlockedCorrosiveRiverTilemapDatas = m_CorrosiveRiverTilemapDatas.ToArray ();
        data.TilemapTriggersAliveStatus = m_TilemapTriggersAliveStatus;
        data.PreservableScreenEntityDatas = m_PreservableEntityDatas;

        return data;
    }

}
