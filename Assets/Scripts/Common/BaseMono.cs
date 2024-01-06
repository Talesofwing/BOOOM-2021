using UnityEngine;

public class BaseMono : MonoBehaviour {

    private GameObject m_GameObject;

    public GameObject CacheGo {
        get {
            if (!m_GameObject) {
                m_GameObject = this.gameObject;
            }

            return m_GameObject;
        }
    }

    private Transform m_Transform;

    public Transform CacheTf {
        get {
            if (!m_Transform) {
                m_Transform = this.transform;
            }

            return m_Transform;
        }
    }

}