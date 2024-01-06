using UnityEngine;

//
// 場景中的物體的基础類
//
public abstract class Entity : BaseMono {
    [SerializeField] private bool m_HideInFirst = false;

    private SpriteRenderer m_Spp;
    
    public virtual void Load () { }

    public virtual void Init () {
        m_Spp = GetComponent<SpriteRenderer> ();

        ResetZByY ();
        
        CacheGo.SetActive (!m_HideInFirst);
    }

    public virtual void Recycle () {
        CacheGo.SetActive (!m_HideInFirst);
    }

    public virtual void Timeout () { }

    public virtual void Remove () {
        CacheGo.SetActive (false);
    }

    public void ResetZByY () {
        CacheTf.position = new Vector3(CacheTf.position.x, CacheTf.position.y, CacheTf.position.y);
    }
    
}
