using UnityEngine;

using Kuma.Utils.Singleton;

public delegate void TimeEventDelegate ();

public class TimeManager : MonoSingleton<TimeManager> {
    private TimeEventElement[] m_TimeEvents;
    
    public bool IsPausing {
        get;
        set;
    }
    
    private float m_CurrentTime;
    public int CurrentTime {
        get { return (int)m_CurrentTime; }
    }

    private int m_PrevTime = 0;

    protected override void Awake () {
        base.Awake ();
        
        IsPausing = false;

        // 數量是0~RecycleTime
        m_TimeEvents = new TimeEventElement[GameSettings.RecycleTime + 1];
    }

    private void Update () {
        if (GameManager.Instance.GameStatus != GameStatus.Gaming || IsPausing)
            return;

        if (m_CurrentTime == 0) {
            DispatchEvent (0);
        }
        
        m_CurrentTime += Time.deltaTime;

        if (m_CurrentTime - m_PrevTime >= 1) {
            m_PrevTime = (int)m_CurrentTime;
            
            // 每隔一秒執行一次新的行動
            int index = (int)m_CurrentTime;
            
            DispatchEvent (index);
        }
    }

    public void Reset () {
        m_CurrentTime = 0;
        m_PrevTime = 0;
    }
    
    private void DispatchEvent (int index) {
        if (index > m_TimeEvents.Length)
            return;
        
        m_TimeEvents[index].Dispatch ();
    }
    
    public void RegisterEvent (int time, TimeEventDelegate handler) {
        if (time > m_TimeEvents.Length) {
            Debug.Log ("[TimeManager]-[RegisterEvent]輸入錯誤，不存在" + time + "秒的時間。");

            return;
        }

        m_TimeEvents[time] += handler;
    }

    public void UnregisterEvent (int time, TimeEventDelegate handler) {
        if (time > m_TimeEvents.Length) {
            Debug.Log ("[TimeManager]-[UnregisterEvent]輸入錯誤，不存在" + time + "秒的時間。");

            return;
        }
        
        m_TimeEvents[time] -= handler;
    }

    private struct TimeEventElement {
        private event TimeEventDelegate TimeEvent;

        public void Dispatch () {
            if (TimeEvent != null) {
                TimeEvent ();
            }
        }

        public static TimeEventElement operator + (TimeEventElement element, TimeEventDelegate handler) {
            element.TimeEvent += handler;
            return element;
        }
        
        public static TimeEventElement operator - (TimeEventElement element, TimeEventDelegate handler) {
            element.TimeEvent -= handler;
            return element;
        }
        
    }
    
}
