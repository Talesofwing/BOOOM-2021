using System.Collections.Generic;
using Game.Common;
using UnityEngine;


public enum SceneType {
    Menu,
    Game,
}

public static class SceneManager {
    private static BaseScene m_CurrentScene;

    private static List<BaseScene> m_Scenes = new List<BaseScene> (); // pool
    private static BaseScene[] m_ScenePrefabs; // prefabs

    public static BaseScene CurrentScene => m_CurrentScene;
    
    public static void LoadAllScenes () {
        m_ScenePrefabs = Resources.LoadAll<BaseScene> (GameConstant.Path.c_RESOURCE_SCENE_PATH);
    }

    // Load new scene and close old scene.
    public static void OpenScene (SceneType type, bool toPool = true, params object[] args) {
        // if isn't the same type of scene.
        if (m_CurrentScene) {
            if (m_CurrentScene.GetSceneType () != type) {
                CloseScene (toPool);
                LoadScene (type, args);
            } else {
                Debug.Log ("[SceneManager OpenScene] : The " + type.ToString () + " scene was already opened");
            }
        } else {
            LoadScene (type, args);
        }
    }

    private static void LoadScene (SceneType type, params object[] args) {
        BaseScene scene = GetSceneInPool (type);
        if (!scene) {
            scene = GetScenePrefab (type);
            scene = Object.Instantiate (scene);
            scene.Close (false);
            m_Scenes.Add (scene);
        }

        m_CurrentScene = scene;
        
        scene.Load (args);
        scene.Open ();
    }

    // Close scene, and push to pool or destroy it.
    public static void CloseScene (bool toPool = true) {
        if (m_CurrentScene) {
            if (toPool) {
                m_CurrentScene.Close (true);
            } else {
                m_Scenes.Remove (m_CurrentScene);
                m_CurrentScene.Destroy ();
            }

            m_CurrentScene = null;
        }
    }

    // Destroy scene in pool.
    public static void DestroyScene (SceneType type) {
        BaseScene scene = GetSceneInPool (type);
        if (scene) {
            scene.Destroy ();
            m_Scenes.Remove (scene);
            Debug.Log ("[SceneManager::DestroyScene] : Destroed the " + type.ToString () + " scene.");
        } else {
            Debug.Log ("[SceneManager::DestroyScene] : There is no" + type.ToString () + " scene in the pool");
        }
    }

    // Get scene in pool.
    private static BaseScene GetSceneInPool (SceneType type) {
        for (int i = 0; i < m_Scenes.Count; i++) {
            if (m_Scenes[i].GetSceneType () == type) {
                return m_Scenes[i];
            }
        }

        return null;
    }

    // Get scene prefab.
    private static BaseScene GetScenePrefab (SceneType type) {
        for (int i = 0; i < m_ScenePrefabs.Length; i++) {
            if (m_ScenePrefabs[i].GetSceneType () == type) {
                return m_ScenePrefabs[i];
            }
        }

        return null;
    }

}