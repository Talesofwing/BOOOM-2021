using System.Collections;
using UnityEngine;

public class MoveTimerEvent : BaseTimerEvent {
    [SerializeField] private Vector2 m_TargetPos;
    [SerializeField] private float m_Duration = 1.0f;
    
    public override void Excute () {
        StartCoroutine (Move ());
    }

    IEnumerator Move () {
        float time = 0.0f;
        Vector3 pos = m_Owner.CacheTf.position;
        Vector3 targetPos = m_TargetPos;
        targetPos.z = pos.z;
        
        while (time < m_Duration) {
            time += Time.deltaTime;

            Vector3 nowPos = Vector3.Lerp (pos, targetPos, time / m_Duration);

            m_Owner.CacheTf.position = nowPos;

            yield return null;
        }
        
        m_Owner.CacheTf.position = targetPos;
        Over ();
    }
    
}
