using System;
using UnityEngine;

public class AnimationDetector : MonoBehaviour {

    public Action AnimationFinished;
    
    public void OnAnimationFinished () {
        if (AnimationFinished != null) {
            AnimationFinished ();
        }
    }
    
}
