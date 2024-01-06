using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapTrigger : ScreenEntity {
    private int m_Id;

    public void Setup (int id, bool isAlive) {
        m_Id = id;

        if (!isAlive) {
            Remove ();
        }
    }

    public override bool Interact () {
        if (base.Interact ()) {
            MapManager.Instance.SetupTilemapTrigger (m_Id);
            
            return true;
        }

        return false;
    }

    public override void Recycle () { }
}
