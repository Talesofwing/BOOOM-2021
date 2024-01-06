using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Common;
using UnityEngine;
using UnityEngine.Serialization;

public static class SaveManager
{
    private static MapData mapSave;
    private static InventoryData inventorySave;
    private static ScenarioData scenarioSave;
    
    public static bool SaveExists
    {
        get
        {
            return File.Exists(GameConstant.Path.c_SAVEDATA_PATH);
        }
    }

    public static bool SaveLoaded {get; private set;}

    public static void LoadGameData()
    {
        if(!SaveExists)
        {
            AudioManager.Instance.SetBGMVolume(1);
            AudioManager.Instance.SetSEVolume(1);
            return;
        }

        string jsonString = File.ReadAllText(GameConstant.Path.c_SAVEDATA_PATH);
        
        var saveData = JsonUtility.FromJson<SaveData>(jsonString);

        mapSave = new MapData()
        {
            PlayerPos = saveData.PlayerPos,
            UnlockedRiverTilemapDatas = saveData.RiverTilemapDatas,
            UnlockedCorrosiveRiverTilemapDatas = saveData.CorrosiveRiverTilemapDatas,
            TilemapTriggersAliveStatus = saveData.TilemapTriggersAliveStatus,
            PreservableScreenEntityDatas = saveData.PreservableScreenEntityDatas,
        };

        inventorySave = new InventoryData()
        {
            itemIds = saveData.itemIds,
            itemCounts = saveData.itemCounts
        };

        scenarioSave = new ScenarioData () {
            HasBeenShowScenarioIds = saveData.HasBeenShowScenarioIds,
        };
        
        AudioManager.Instance.SetBGMVolume(saveData.bgmVol);
        AudioManager.Instance.SetSEVolume(saveData.seVol);

        SaveLoaded = true;
    }

    public static MapData GetMapSave()
    {
        return mapSave;
    }

    public static InventoryData GetInventorySave()
    {
        return inventorySave;
    }

    public static ScenarioData GetScenarioSave () {
        return scenarioSave;
    }
    
    public static void SaveGameData()
    {
        mapSave = MapManager.Instance.GetSaveData();
        inventorySave = InventoryManager.Instance.GetInventorySave();
        scenarioSave = ScenarioManager.Instance.GetSaveData ();
        
        var gameData = new SaveData()
        {
            PlayerPos = mapSave.PlayerPos,
            RiverTilemapDatas = mapSave.UnlockedRiverTilemapDatas,
            CorrosiveRiverTilemapDatas = mapSave.UnlockedCorrosiveRiverTilemapDatas,
            TilemapTriggersAliveStatus = mapSave.TilemapTriggersAliveStatus,
            PreservableScreenEntityDatas = mapSave.PreservableScreenEntityDatas,
            HasBeenShowScenarioIds = scenarioSave.HasBeenShowScenarioIds,
            itemIds = inventorySave.itemIds,
            itemCounts = inventorySave.itemCounts,
            bgmVol = AudioManager.Instance.BGMVol,
            seVol = AudioManager.Instance.SEVol,
        };

        string jsonString = JsonUtility.ToJson(gameData);
        // Write JSON to file.
        File.WriteAllText(GameConstant.Path.c_SAVEDATA_PATH, jsonString);
        
        AnalyticsManager.Track(AnalyticsEventKey.Save);
    }

    public static void DelectSave()
    {
        if(SaveExists)
        {
            File.Delete (GameConstant.Path.c_SAVEDATA_PATH);
        }        
    }

    [Serializable]
    public class SaveData
    {
        public Vector3 PlayerPos;
        public MapData.TileData[] RiverTilemapDatas;
        public MapData.TileData[] CorrosiveRiverTilemapDatas;
        public MapData.ScreenEntityData[] PreservableScreenEntityDatas;
        public int[] HasBeenShowScenarioIds;
        public bool[] TilemapTriggersAliveStatus;
        public int[] itemIds;
        public int[] itemCounts;
        public float bgmVol = 1;
        public float seVol = 1;
    }

}
