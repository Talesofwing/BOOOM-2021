using System.Collections.Generic;
using Kuma.Utils.Singleton;

public enum ModuleType {
    Game
}

public class ModuleManager : Singleton<ModuleManager> {
    private List<BaseModule> m_Modules = new List<BaseModule> ();
    
    public void Init () {
        m_Modules.Add (new GameModule());
    }

    public T FindModule<T> () where T : BaseModule {
        for (int i = 0; i < m_Modules.Count; i++) {
            if (m_Modules[i] is T) {
                return (T)m_Modules[i];
            }
        }

        return null;
    }
   
}
