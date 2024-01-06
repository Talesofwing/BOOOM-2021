public enum UIStatus {
    Initializing, // 初始化中
    Loading, // 在緩存池中重新打開
    Opening, // 正在顯示
    Closing, // 已隱藏
    Destroyed // 已銷毀
}

public abstract class BaseUI : BaseMono {
    public abstract UIType GetUIType ();

    public UIStatus Status { get; protected set; }

    protected virtual void Awake () {
        Status = UIStatus.Initializing;
    }

    public virtual void Load (params object[] args) {
        Status = UIStatus.Loading;
    }

    public virtual void Open () {
        Status = UIStatus.Opening;

        Show ();
    }

    // 刪除動態的objects
    // destroyRes : if true, destroy all dynamic reosurces.
    public virtual void Close () {
        Status = UIStatus.Closing;

        Hide ();
    }

    // 刪除所有東西
    // 因為由SceneManager刪除具體的GameObject
    // 所以做一些保存操作便可
    public virtual void Destroy () {
        Status = UIStatus.Destroyed;

        // Destroy前會調用一次Close
        Close ();
        
        DestroyData ();

        Destroy (CacheGo);
    }

    protected virtual void DestroyData () { }

    protected void Show () {
        CacheGo.SetActive (true);
    }

    protected void Hide () {
        CacheGo.SetActive (false);
    }

}