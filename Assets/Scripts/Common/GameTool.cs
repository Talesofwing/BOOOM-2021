using Kuma.Utils.Singleton;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTool : MonoSingleton<GameTool> {
    [SerializeField] private Item2TileInf[] m_Item2TileInfs;

    public static TileBase getTileOfItem (int id) {
        return Instance.GetTileOfItem (id);
    }

    public TileBase GetTileOfItem (int id) {
        for (int i = 0; i < m_Item2TileInfs.Length; i++) {
            if (id == m_Item2TileInfs[i].Item.ID) {
                return m_Item2TileInfs[i].Tile;
            }
        }

        return null;
    }
    
    [System.Serializable]
    private struct Item2TileInf {
        public ItemData Item;
        public TileBase Tile;
    }
    
}
