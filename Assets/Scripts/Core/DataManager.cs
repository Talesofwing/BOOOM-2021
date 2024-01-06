using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kuma.Utils.Singleton;
using System.Linq;
using Game.Common;

public static class DataManager
{
    private static ItemData[] m_ItemDataList;
    public static ItemData[] AllItemData => m_ItemDataList;
    private static ScreenEntityData[] m_EntityDataDataList;


    public static void LoadAllData () 
    {
        m_ItemDataList = Resources.LoadAll<ItemData> (GameConstant.Path.c_RESOURCE_ItemData_PATH);
        m_EntityDataDataList = Resources.LoadAll<ScreenEntityData> (GameConstant.Path.c_RESOURCE_EntityData_PATH);
    }

    public static ItemData GetItemData(int index)
    {
        return m_ItemDataList.FirstOrDefault(item => item.ID == index);
    }

    public static ScreenEntityData GetEntityDataData(int index)
    {
        return m_EntityDataDataList.FirstOrDefault(entity => entity.ID == index);
    }
    
}
