/*
    IUpdatable.cs
    Author: Kuma
    Created-Date: 2020/03/04
    
    -----Description-----
    Update interface.
*/






namespace Kuma.Utils.UpdateManager {

    public interface IUpdatable {
        void CallUpdate (float deltaTime);
    }

    public interface IFixedUpdatable {
        void CallFixedUpdate (float fixedTime);
    }

    public interface ILateUpdatable {
        void CallLateUpdate (float deltaTime);
    }
    
}