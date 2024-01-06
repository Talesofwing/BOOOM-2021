using UnityEngine;

public class SelectedItemUI : BaseUI {

    private Vector2 m_StartingScale;
    
    public override UIType GetUIType () {
        return UIType.SelectedItemUI;
    }

    protected override void Awake () {
        m_StartingScale = CacheTf.localScale;
    }

    public override void Open () {
        base.Open ();

        Hide ();
        
        ModuleManager.Instance.FindModule<GameModule> ().OnInteractionChanged += OnInteractionChanged;
    }

    public override void Close () {
        base.Close ();

        ModuleManager.Instance.FindModule<GameModule> ().OnInteractionChanged -= OnInteractionChanged;
    }

#region Events

    private void OnInteractionChanged (Interactor.InteractableElement? element) {
        if (element == null) {
            Hide ();
        } else {
            Show ();
            
            Vector2 scale = element.Value.Obj.GetUIScale ();
            Vector2 offset = element.Value.Obj.GetUIOffset ();
            CacheTf.localScale = m_StartingScale * scale;

            switch (element.Value.Obj.GetInteractableType ()) {
                case InteractableType.Tilemap:
                    float xOffset = scale.x / 2.0f - 0.5f;
                    float yOffset = scale.y / 2.0f - 0.5f;
                    Vector2 dir = MapManager.Instance.CachePlayer.VectorDirection;
                    dir.x *= xOffset;
                    dir.y *= yOffset;

                    CacheTf.position = MapManager.Instance.GetWorldPosByTileInFrontOfPlayer () + dir + new Vector2 (0.5f, 0.5f) + offset;
                    break;
                case InteractableType.ScreenObject:
                    CacheTf.position = element.Value.Obj.GetPosition () + (Vector3)offset;
                    break;
            }
            
        }
    }

#endregion

}
