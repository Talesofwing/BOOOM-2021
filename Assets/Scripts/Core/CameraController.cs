using Game.Common;
using Kuma.Utils.Singleton;
using Kuma.Utils.UpdateManager;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>, ILateUpdatable {
    [SerializeField] private bool m_StopFollow = false;
    [SerializeField] private float m_SmoothSpeed = 20.0f;
    
    private Transform m_Target;
    private Transform m_CacheTf;
    private Vector3 m_Offset;
    private Vector3 m_StartingPos;
    
    public bool IsFollowing {
        get;
        private set;
    }
    
    protected override void Awake () {
        base.Awake();
        
        m_CacheTf = this.transform;
        m_StartingPos = m_CacheTf.position;
    }

    public void SetTarget (Transform target) {
        m_Target = target;
        
        if (target)
            m_Offset = m_StartingPos - m_Target.position;
    }
    
    public void StartFollow () {
        if (!IsFollowing) {
            UpdateDispatcher.Instance.AddLateUpdatable (this);
            IsFollowing = true;
        }
    }
    
    public void CancelFollow () {
        if (IsFollowing) {
            UpdateDispatcher.Instance.RemoveLateUpdatable (this);
            IsFollowing = false;
        }
    }

    public void ResetPosition () {
        m_CacheTf.position = m_StartingPos;
    }

    public void MoveToTarget () {
        m_CacheTf.position = m_Target.position + m_Offset;
    }
    
    public void CallLateUpdate (float deltaTime) {
        if (m_StopFollow)
            return;
        
        Vector3 movement = Vector3.Lerp (m_CacheTf.position, m_Target.position + m_Offset, Time.deltaTime * m_SmoothSpeed);
        movement.z = m_StartingPos.z;
        m_CacheTf.position =  movement;
    }
    
}
