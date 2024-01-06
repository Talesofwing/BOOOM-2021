public enum SceneStatus {
    Initializing, // 初始化中
    Loading, // 在緩存池中重新打開
    Opening, // 正在顯示
    Closing, // 已隱藏
    Destroyed // 已銷毀
}

public abstract class BaseScene : BaseMono {
    public abstract SceneType GetSceneType ();

    public SceneStatus Status { get; protected set; }

    protected void Awake () {
        Status = SceneStatus.Initializing;
    }

    // 讀取及初始化場景中的objects
    public virtual void Load (params object[] args) {
        Status = SceneStatus.Loading;
    }

    public virtual void Open () {
        Status = SceneStatus.Opening;

        Show ();
    }

    // 刪除動態的objects
    // destroyRes : if true, destroy all dynamic reosurces.
    public virtual void Close (bool destroyRes) {
        Status = SceneStatus.Closing;

        Hide ();
    }

    // 刪除所有東西
    // 因為由SceneManager刪除具體的GameObject
    // 所以做一些保存操作便可
    public virtual void Destroy () {
        Status = SceneStatus.Destroyed;

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