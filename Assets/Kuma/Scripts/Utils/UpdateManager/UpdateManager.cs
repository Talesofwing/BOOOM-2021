/*
    UpdateManager.cs
    Author: Kuma
    Created-Date: 2020/03/04
    
    -----Description-----
    Update manager.  
*/




using System.Collections.Generic;

namespace Kuma.Utils.UpdateManager {

    public class UpdateManager : IUpdateManager {
        private List<IUpdatable> _updatables;
        private List<IFixedUpdatable> _fixedUpdatables;
        private List<ILateUpdatable> _lateUpdatables;

        public string Name { get; set; }

        public UpdateManager () {
            _updatables = new List<IUpdatable> ();
            _fixedUpdatables = new List<IFixedUpdatable> ();
            _lateUpdatables = new List<ILateUpdatable> ();
        }

        public UpdateManager (string name) {
            Name = name;

            _updatables = new List<IUpdatable> ();
            _fixedUpdatables = new List<IFixedUpdatable> ();
            _lateUpdatables = new List<ILateUpdatable> ();
        }

        public void Update (float deltaTime) {
            int count = _updatables.Count;
            for (int i = 0; i < count; i++) {
                _updatables[i].CallUpdate (deltaTime);
            }
        }

        public void FixedUpdate (float fixedTime) {
            int count = _fixedUpdatables.Count;
            for (int i = 0; i < count; i++) {
                _fixedUpdatables[i].CallFixedUpdate (fixedTime);
            }
        }

        public void LateUpdate (float deltaTime) {
            int count = _lateUpdatables.Count;
            for (int i = 0; i < count; i++) {
                _lateUpdatables[i].CallLateUpdate (deltaTime);
            }
        }

        public void Add (IUpdatable updatable) {
            _updatables.Add (updatable);
        }

        public void Add (IFixedUpdatable fixedUpdatable) {
            _fixedUpdatables.Add (fixedUpdatable);
        }

        public void Add (ILateUpdatable lateUpdatable) {
            _lateUpdatables.Add (lateUpdatable);
        }

        public void Remove (IUpdatable updatable) {
            _updatables.Remove (updatable);
        }

        public void Remove (IFixedUpdatable fixedUpdatable) {
            _fixedUpdatables.Remove (fixedUpdatable);
        }

        public void Remove (ILateUpdatable lateUpdatable) {
            _lateUpdatables.Remove (lateUpdatable);
        }

        public void ClearAll () {
            _updatables.Clear ();
            _fixedUpdatables.Clear ();
            _lateUpdatables.Clear ();
        }

    }

}