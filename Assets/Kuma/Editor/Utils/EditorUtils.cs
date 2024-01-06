/*
    EditorUtils.cs
    Author: Kuma
    Created-Date: 2019-02-07

    -----Description-----
    Unity編輯器的擴展小功能
*/





using UnityEngine;
using UnityEditor;

namespace Kuma.Editor.Utils {

    public class EditorTools {
        
        [MenuItem ("Kuma/Utils/Inversion Active %&a", true)]
        private static bool InversionGameObjectActiveValidateFunction () {
            GameObject[] gos = Selection.gameObjects;
            return gos != null && gos.Length > 0;        
        }

        [MenuItem ("Kuma/Utils/Inversion Active %&a")]
        private static void InversionGameObjectActive () {
            GameObject[] gos = Selection.gameObjects;
            int length = gos.Length;
            for (int i = 0; i < length; i++) {
                GameObject go = gos [i];
                bool active = go.activeSelf;
                go.SetActive (!active);
            }
        }

        [MenuItem ("Kuma/Utils/Clear PlayerPrefs %&q")]
        private static void ClearPlayerPrefas () {
            PlayerPrefs.DeleteAll ();
            Debug.Log ("PlayerPrefs Clear.");
            SaveManager.DelectSave();
        }

    }

}