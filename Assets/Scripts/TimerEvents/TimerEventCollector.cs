using System.Collections.Generic;

public class TimerEventCollector : BaseMono {
    private List<BaseTimerEvent> m_TimerEvents = new List<BaseTimerEvent> ();
    private TimerEventFamily[] m_TimerEventFamilies;
    
    public void Init (Entity owner) {
        foreach (var e in GetComponents<BaseTimerEvent> ()) {
            if (!e.IsFamilyEvent) {
                m_TimerEvents.Add(e);
            }
        }
        
        m_TimerEventFamilies = GetComponents<TimerEventFamily> ();
        
        foreach (var e in m_TimerEvents) {
            e.Init (owner);
        }

        foreach (var family in m_TimerEventFamilies) {
            family.Init (owner);
        }
    }

    public void Recycle () {
        foreach (var e in m_TimerEvents) {
            e.Recycle ();
        }
        
        foreach (var family in m_TimerEventFamilies) {
            family.Recycle ();
        }
    }

}
