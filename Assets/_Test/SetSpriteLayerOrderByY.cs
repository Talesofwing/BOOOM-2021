using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteLayerOrderByY : MonoBehaviour {
    [SerializeField] private SpriteRenderer m_SRR;
    [SerializeField] private float m_Height;
    
    private Transform m_tf;

    private void Start () {
        m_tf = this.transform;
    }
    
    private void Update () {
        float y = m_tf.position.y;
        y += m_Height;

        m_SRR.sortingOrder = -(int)y;
    }
    
}
