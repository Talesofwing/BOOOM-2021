using UnityEngine;

public class TimerEventFamily : MonoBehaviour {
    [Tooltip("最先執行的")]
    [SerializeField] private BaseTimerEvent m_HeadTimerEvent;
    [SerializeField] private BaseTimerEvent[] m_TimerEvents;
    
    public void Init (Entity owner) {
        m_HeadTimerEvent.Init (owner);
        m_HeadTimerEvent.Finished += () => {
            m_TimerEvents[0].Excute ();
        };
        m_HeadTimerEvent.RegisterEvent ();
        
        for (int i = 0; i < m_TimerEvents.Length; ++i) {
            m_TimerEvents[i].Init (owner);

            if (i < m_TimerEvents.Length - 1) {
                int index = i + 1;
                m_TimerEvents[i].Finished += () => {
                    m_TimerEvents[index].Excute ();
                };
            }
        }
    }

    public void Recycle () {
        m_HeadTimerEvent.Recycle ();
        
        for (int i = 0; i < m_TimerEvents.Length; ++i) {
            m_TimerEvents[i].Recycle ();
        }
    }

}
