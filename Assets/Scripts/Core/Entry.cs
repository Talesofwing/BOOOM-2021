using System;
using Kuma.Utils.UpdateManager;
using UnityEngine;

//
// App的初始化
//

public class Entry : MonoBehaviour {
    public SceneType LoadSceneFirst;
    
    private void Start () {
        SceneManager.LoadAllScenes ();
        DataManager.LoadAllData();
        UIManager.LoadAllUIs ();
        SaveManager.LoadGameData ();

        // 創建默認的UpdateManager
        UpdateDispatcher.Instance.Create ();
        // Module
        ModuleManager.Instance.Init ();

        SceneManager.OpenScene (LoadSceneFirst, true, true);
    }

    private void OnApplicationQuit () {
        double endTime = Time.realtimeSinceStartupAsDouble;
        AnalyticsManager.Track (AnalyticsEventKey.GameQuit, AnalyticsManager.GetDictionary("run_time", endTime));
    }
}
