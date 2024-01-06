/*
    Singleton.cs
    Author: Kuma
    Created-Date: 2019/03/10

    -----Description-----
    Class singleton.
*/ 





using UnityEngine;

namespace Kuma.Utils.Singleton {

    public abstract class Singleton<T> where T : class, new() {
        private static T _instance;
        public static T Instance {
            get { return GetInstance (); }
        }

        public static T GetInstance () {
            if (_instance == null) {
                _instance = new T();
            }
            return _instance;
        }

    }

}