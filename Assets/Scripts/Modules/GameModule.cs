using System;

public delegate void InteractionChangedEventHandler (Interactor.InteractableElement? nearstElement);

public class GameModule : BaseModule {
    public event InteractionChangedEventHandler OnInteractionChanged;

    private Interactor.InteractableElement? m_Element;
    public Interactor.InteractableElement? NearstElement {
        get {
            return m_Element;
        }
        set {
            m_Element = value;
            if (OnInteractionChanged != null)
                OnInteractionChanged (value);
        }
    }

    public override ModuleType GetModuleType () {
        return ModuleType.Game;
    }
}
