using System.Collections;
using System.Collections.Generic;
using Game.Common;
using Kuma.Utils.Singleton;
using UnityEngine;

public class ScenarioManager : Singleton<ScenarioManager> {
    private Scenario[] m_ScenarioDatas;
    private List<int> m_HasBeenShowScenarioIds;

    public void Load (bool isNewGame) {
        m_ScenarioDatas = Resources.LoadAll<Scenario> (GameConstant.Path.c_RESOURCE_SCENARIODATA_PATH);

        if (isNewGame) {
            m_HasBeenShowScenarioIds = new List<int> ();
        } else {
            int[] ids = SaveManager.GetScenarioSave ().HasBeenShowScenarioIds;
            m_HasBeenShowScenarioIds = new List<int> (ids);
        }
    }

    public Scenario GetScenarioDataById (int id) {
        foreach (var data in m_ScenarioDatas) {
            if (id == data.ID) {
                return data;
            }
        }

        return null;
    }

    public bool CheckHasBeenShowById (int id) {
        for (int i = 0; i < m_HasBeenShowScenarioIds.Count; i++) {
            if (id == m_HasBeenShowScenarioIds[i]) {
                return true;
            }
        }

        return false;
    }

    public void AddHasBeenShowScenarioId (int id) {
        m_HasBeenShowScenarioIds.Add (id);
    }
    
    public ScenarioData GetSaveData () {
        ScenarioData data;
        data.HasBeenShowScenarioIds = m_HasBeenShowScenarioIds.ToArray ();
        return data;
    }
    
}
