using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractionCondition : MonoBehaviour {
    
    public virtual bool CheckInteractable () {
        return true;
    }

}
