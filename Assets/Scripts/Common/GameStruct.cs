//
// 道具信息
//
[System.Serializable]
public struct ItemInf {
    public ItemData Data; // 道具資料
    public int Count;     // 道具數量

    public int ItemId {
        get { return Data.ID; }
    }
    
}