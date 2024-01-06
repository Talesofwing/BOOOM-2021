using System;
using UnityEngine;

public class SetActivityTimerEvent : BaseTimerEvent {
    [SerializeField] private bool m_Active = true;

    public override void Excute () {
        m_Owner.CacheGo.SetActive (m_Active);
        
        base.Excute ();
    }

}
