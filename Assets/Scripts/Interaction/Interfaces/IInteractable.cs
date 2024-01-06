using UnityEngine;

public enum InteractableType {
    ScreenObject,
    Tilemap,
}

public interface IInteractable {
    public InteractableType GetInteractableType ();
    public Vector3 GetPosition ();
    public bool Interact ();
    public void Remove ();
    public Vector2 GetUIScale ();
    public Vector2 GetUIOffset ();
}
