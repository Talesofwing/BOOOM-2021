using Kuma.Utils.Singleton;
using UnityEngine;

public class GameSettings : MonoSingleton<GameSettings> {
    [SerializeField] private int m_RecycleTime = 60;
    public static int RecycleTime => Instance.m_RecycleTime;

}
