using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioTrigger : Entity {
    [SerializeField] private Scenario m_Scenario;
    [SerializeField] private bool m_IsBlackBackground = false;

    private void OnTriggerEnter2D (Collider2D other) {
        GameManager.Instance.GamePause ();

        DialogUI ui = UIManager.OpenUI (UIType.DialogUI, m_Scenario, m_IsBlackBackground) as DialogUI;
        ui.Exit += () => {
            GameManager.Instance.GameContinue ();
        };
        
        MapManager.Instance.SetupPreservableEntityStatus (this, false);
        Remove ();
    }
    
}
