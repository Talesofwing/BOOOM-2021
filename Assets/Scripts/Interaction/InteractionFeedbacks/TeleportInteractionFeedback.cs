using UnityEngine;

public class TeleportInteractionFeedback : BaseInteractionFeedback {
    [SerializeField] private Vector3 m_EntryPos;
    [SerializeField] private Vector3 m_TargetPos;

    private bool m_InEntry = true;
            
    public override void Excute () {
        if (m_InEntry) {
            MapManager.Instance.CachePlayer.CacheTf.position = m_TargetPos;
        } else {
            MapManager.Instance.CachePlayer.CacheTf.position = m_EntryPos;
        }

        m_InEntry = !m_InEntry;
    }
    
}
