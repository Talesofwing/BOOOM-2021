using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseTimerEvent : MonoBehaviour {
    [SerializeField] protected bool m_IsFamilyEvent = false;
    
    [Tooltip("觸發的時間")]
    [SerializeField] protected int m_TriggerTime;

    public Action Finished;
    
    protected Entity m_Owner;

    public bool IsFamilyEvent => m_IsFamilyEvent;
    
    public virtual void Init (Entity owner) {
        m_Owner = owner;

        if (!m_IsFamilyEvent)
            RegisterEvent ();
    }

    public void RegisterEvent () {
        TimeManager.Instance.RegisterEvent (m_TriggerTime, Excute);
    }
    
    public virtual void Recycle () { }

    public virtual void Excute () {
        Over ();
    }

    protected virtual void Over () {
        if (Finished != null)
            Finished ();
    }
    
}
