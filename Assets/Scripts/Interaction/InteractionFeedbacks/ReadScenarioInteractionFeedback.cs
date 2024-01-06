using UnityEngine;

public class ReadScenarioInteractionFeedback : BaseInteractionFeedback {
    [SerializeField] private Scenario m_Scenraio;
    
    public override void Excute () {
        GameManager.Instance.GamePause ();
        DialogUI ui = (DialogUI)UIManager.OpenUI (UIType.DialogUI, m_Scenraio, false);
        ui.Exit += () => {
            GameManager.Instance.GameContinue();
        };
        
                
        AnalyticsManager.Track(AnalyticsEventKey.Interact, AnalyticsManager.GetDictionary("type", "ReadScenario"));
        AnalyticsManager.Track(AnalyticsEventKey.ShowScenario, AnalyticsManager.GetDictionary("id", m_Scenraio.ID));
    }
    
}
