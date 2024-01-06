using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventCollector : BaseMono {
    [SerializeField] private Interaction[] m_Interaction;
    [SerializeField] private string m_InteractionFailureText = "交互失败";
    [SerializeField] private string m_ExecutionFailureText = "背包已满";
    
    private IInteractable m_Owner;

    public void Init (IInteractable owner) {
        m_Owner = owner;
    }

    public bool Interact () {
        Interaction? interaction;
        if (CheckIntractable (out interaction)) {
            if (CheckExecutable (interaction.Value)) {
                interaction.Value.Excute ();

                if (interaction.Value.AutoRemove) {
                    m_Owner.Remove ();
                }
                
                return true;
            } else {
                if (m_InteractionFailureText != "")
                    UIManager.OpenUI (UIType.TipUI, m_ExecutionFailureText);
            }
        } else {
            if (m_ExecutionFailureText != "")
                UIManager.OpenUI (UIType.TipUI, m_InteractionFailureText);
        }

        return false;
    }

    //
    // 判斷交互條件
    //
    private bool CheckIntractable (out Interaction? interaction) {
        // 順序判斷每一個Interaction expression
        // 如果有其中一個是success的話，就跳出迴圈執行對應的feedback

        interaction = null;
        bool success = true;
        foreach (var e in m_Interaction) {
            success = true;
            
            foreach (var condition in e.Conditions) {
                if (!condition.CheckInteractable ()) {
                    success = false;
                }
            }
    
            // 其中一種條件成功的話，就執行那個條件的Feedback.
            if (success) {
                interaction = e;
                return true;
            }
        }

        return success;
    }

    //
    // 判斷交互是否成功
    //
    private bool CheckExecutable (Interaction interaction) {
        bool success = true;
        foreach (var feedback in interaction.Feedbacks) {
            if (!feedback.CheckExecutable ()) {
                success = false;
            }
        }

        return success;
    }
    
    [System.Serializable]
    public struct Interaction {
        public bool AutoRemove;
        public BaseInteractionCondition[] Conditions;
        public BaseInteractionFeedback[] Feedbacks;
        
        public void Excute () {
            foreach (var e in Feedbacks) {
                e.Excute ();
            }
        }
    }

}
