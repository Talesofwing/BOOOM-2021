using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractionFeedback : MonoBehaviour {

    public virtual bool CheckExecutable () {
        return true;
    }

    public abstract void Excute ();
}
