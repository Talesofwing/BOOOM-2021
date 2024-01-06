/*
    MonoSingleton.cs
    Author: Kuma
    Created-Date: 2019-03-08

    -----Description-----
    MonoBehaviour singleton object.
*/





using UnityEngine;

namespace Kuma.Utils.Singleton {

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        [SerializeField] protected bool _dontDestroyOnLoad = true;

        private static bool _applicationIsQuit = false;
        private static object _lock = new object();

        private static T _instance;
        public static T Instance {
            get { return GetInstance (); }
        }

        public static T GetInstance () {
            if (_applicationIsQuit) {
                return null;
            }
            
            lock (_lock) {
                if (_instance == null) {
                    _instance = FindObjectOfType<T> (true);

                    if (_instance == null) {
                        GameObject singleton = new GameObject (typeof(T).ToString ());
                        _instance = singleton.AddComponent<T> ();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake () {
            _instance = this as T;
            if (_dontDestroyOnLoad) {
                DontDestroyOnLoad (_instance.gameObject);
            }
        }

        protected virtual void OnApplicationQuit() {
            _applicationIsQuit = true;
        }

    }

}