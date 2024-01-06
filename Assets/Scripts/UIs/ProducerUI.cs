using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProducerUI : BaseUI {
    
    public override UIType GetUIType () {
        return UIType.ProducerUI;
    }

    public void CloseButtonClick () {
        UIManager.CloseUI (this);
    }
    
}
