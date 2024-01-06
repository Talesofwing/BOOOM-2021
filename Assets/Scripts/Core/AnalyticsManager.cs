using System.Collections.Generic;
using UnityEngine.Analytics;

/// <summary>
/// 所有埋点的Key。方便快速追踪是否被调用，在哪调用。
/// </summary>
public struct AnalyticsEventKey
{
    public const string GameStart = "game_start";
    public const string GameQuit = "game_quit";
    public const string VisitScreen = "visit_screen";
    public const string Interact = "interact";
    public const string GetItems = "get_items";
    public const string CraftItems = "craft_items";
    public const string PutItems = "put_items";
    public const string ShowScenario = "show_scenario";
    public const string Clear = "clear";
    public const string Save = "save";
}

public static class AnalyticsManager {

    /// <summary>
    /// 上传事件数据
    /// </summary>
    /// <param name="eventName">事件key</param>
    /// <param name="properties">事件属性，字典类型（属性key、属性变量）</param>
    public static void Track(string eventName, Dictionary<string, object> properties = null)
    {
        switch (eventName)
        {
            case AnalyticsEventKey.GameStart:
                AnalyticsEvent.GameStart(properties);   // 使用标准事件
                break;
            case AnalyticsEventKey.VisitScreen:
                AnalyticsEvent.ScreenVisit(properties["screen"].ToString());    // 使用标准事件
                break;
            case AnalyticsEventKey.Interact:
                // 不上报交互事件，减少上报量
                break;
            default:
                AnalyticsEvent.Custom(eventName, properties);
                break;
        }
    }

    public static Dictionary<string, object> GetDictionary (string key, object value) {
        Dictionary<string, object> dict = new Dictionary<string, object> ();
        dict[key] = value;

        return dict;
    }
    
}
