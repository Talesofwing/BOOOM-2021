using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game.Common {

    public static class GameConstant {

        public static class Path {
            public const string c_RESOURCE_SCENE_PATH = "Scenes/";
            public const string c_RESOURCE_UI_PATH = "UIs/";
            public const string c_RESOURCE_ItemData_PATH = "GameData/ItemData";
            public const string c_RESOURCE_EntityData_PATH = "GameData/EntityData";
            public const string c_RESOURCE_SCENARIODATA_PATH = "GameData/ScenarioDatas";
            
#if UNITY_EDITOR
            public static string c_SAVEDATA_PATH => Application.persistentDataPath + "/saveData.json";
#else
            public static string c_SAVEDATA_PATH => System.AppDomain.CurrentDomain.BaseDirectory + "/saveData.json";
#endif
        }

        public const float EPSILON = 0.0001f;
    }

}