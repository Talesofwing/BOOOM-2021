using System;
using System.Collections.Generic;
using UnityEngine;


public class Interactor : BaseMono {
    [SerializeField] private LayerMask m_InteractiveLayer;

    [Header ("碰撞體")] 
    [SerializeField] private Vector2 m_ColliderSize = new Vector2 (0.5f, 0.5f);
    [SerializeField] private float m_ColliderOffset = 0.2f;
    
    private List<InteractableElement> m_Interactions = new List<InteractableElement> ();

    public bool HasInteractable => m_Interactions.Count > 0;

    public void Reset () {
        m_Interactions.Clear ();
    }

    public bool Check (Vector2 dir) {
    #if UNITY_EDITOR
        m_Dir = dir;
    #endif

        Reset ();

        RaycastHit2D[] hits;
        bool anyHit = false;
        hits = Physics2D.BoxCastAll ((Vector2)CacheTf.position + dir * m_ColliderOffset, m_ColliderSize, 0.0f,
                                     Vector2.zero, 0.0f, m_InteractiveLayer);
        if (hits.Length > 0) {
            for (int i = 0; i < hits.Length; ++i) {
                RaycastHit2D hit = hits[i];
                IInteractable obj = hit.collider.GetComponent<IInteractable> ();
                if (obj != null) {
                    InteractableElement e;
                    e.Obj = obj;
                    e.HitPoint = hit.point;
                    m_Interactions.Add (e);

                    anyHit = true;
                }
            }
        }
        
        return anyHit;
    }

    public InteractableElement GetNearest (Vector2 pos) {
        int count = m_Interactions.Count;

        if (count <= 0)
            return InteractableElement.Null;

        if (count == 1)
            return m_Interactions[0];

        int index = 0;
        float distance = float.MaxValue;
        for (int i = 0; i < count; ++i) {
            Vector2 objPos = m_Interactions[i].HitPoint;
            float tempDeistance = Vector2.Distance (objPos, pos);
            if (tempDeistance <= distance) {
                index = i;
                distance = tempDeistance;
            }
        }

        return m_Interactions[index];

    }

#if UNITY_EDITOR
    private Vector2 m_Dir;

    private void OnDrawGizmosSelected () {
        Gizmos.color = Color.red;
        if (!Application.isPlaying) {
            Gizmos.DrawWireCube ((Vector2)CacheTf.position + Vector2.right * m_ColliderOffset, m_ColliderSize);
        } else {
            Gizmos.DrawWireCube ((Vector2)CacheTf.position + m_Dir * m_ColliderOffset, m_ColliderSize);
        }
    }
#endif

    [Serializable]
    public struct InteractableElement {
        public IInteractable Obj;
        public Vector2 HitPoint;

        public static InteractableElement Null {
            get { return new InteractableElement (); }
        }
    }

}
