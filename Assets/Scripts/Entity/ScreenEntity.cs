using UnityEngine;

[RequireComponent (typeof (InteractionEventCollector))]
[RequireComponent (typeof (TimerEventCollector))]
public abstract class ScreenEntity : Entity, IInteractable {
    [SerializeField] private InteractableType m_InteractableType;
    [SerializeField] private ScreenEntityData m_ScreenEntityData;
    [SerializeField] private Vector2 m_UIScale = new Vector2 (1.0f, 1.0f);
    [SerializeField] private Vector2 m_UIOffset = new Vector2 (0.0f, 0.0f);
    
    protected TimerEventCollector m_TimerEventCollector;
    protected InteractionEventCollector m_InteractionEventCollerctor;

    private Vector3 m_StartingPosition;

    public ScreenEntityData Data => m_ScreenEntityData;
    
#region 交互實現

    public InteractableType GetInteractableType () {
        return m_InteractableType;
    }

    public Vector3 GetPosition () {
        return CacheTf.position;
    }

    public virtual bool Interact () {
        if (m_InteractionEventCollerctor.Interact ()) {
            return true;
        }

        return false;
    }

#endregion

#region 實體狀態

    public Vector2 GetUIScale () {
        return m_UIScale;
    }

    public Vector2 GetUIOffset () {
        return m_UIOffset;
    }
    
    protected virtual void Awake () {
        MapManager.Instance.RegisterEntity (this);
    }

    public override void Init () {
        base.Init ();

        m_InteractionEventCollerctor = GetComponent<InteractionEventCollector> ();
        m_TimerEventCollector = GetComponent<TimerEventCollector> ();

        m_TimerEventCollector.Init (this);
        m_InteractionEventCollerctor.Init (this);

        m_StartingPosition = CacheTf.position;
    }

    public override void Recycle () {
        m_TimerEventCollector.Recycle ();

        CacheTf.position = m_StartingPosition;
        
        base.Recycle ();
    }

#endregion

}