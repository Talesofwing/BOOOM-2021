/*
    IUpdateManager.cs
    Author: Kuma
    Created-Date: 2020/03/04
    
    -----Description-----
    UpdateManager interface.
*/





namespace Kuma.Utils.UpdateManager {
    
    public interface IUpdateManager {
        string Name { get; set; }

        void Update (float deltaTime);
        void FixedUpdate (float fixedTime);
        void LateUpdate (float deltaTime);

        void Add (IUpdatable updatable);
        void Add (IFixedUpdatable fixedUpdatable);
        void Add (ILateUpdatable lateUpdatable);

        void Remove (IUpdatable updatable);
        void Remove (IFixedUpdatable fixedUpdatable);
        void Remove (ILateUpdatable lateUpdatable);

        void ClearAll ();
    }

}