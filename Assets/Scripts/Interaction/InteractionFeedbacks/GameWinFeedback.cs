using UnityEngine;

public class GameWinFeedback : BaseInteractionFeedback {
    [SerializeField] private GameWinType m_WinType;
    
    public override void Excute () {
        GameManager.Instance.GameWin (m_WinType);
    }
    
}
