/*
    UpdateDispatcher.cs
    Author: Kuma
    Created-Date: 2020/03/04
    
    -----Description-----
    觸發Update.
*/





using System.Collections.Generic;

using UnityEngine;

using Kuma.Utils.Singleton;

namespace Kuma.Utils.UpdateManager {

    public class UpdateDispatcher : MonoSingleton<UpdateDispatcher> {
        private List<UpdateManager> _managers = new List<UpdateManager> ();
        private List<UpdateManager> _run = new List<UpdateManager> ();

        private void Update () {
            float dt = Time.deltaTime;
            int count = _run.Count;
            for (int i = 0; i < count; i++) {
                UpdateManager manager = _run[i];
                if (null == manager) {
                    // If the manager is null.
                    // Delete it and the index--.
                    _run.RemoveAt (i--);
                    continue;
                }
                manager.Update (dt);
            }
        }

        private void FixedUpdate () {
            float fdt = Time.fixedDeltaTime;
            int count = _run.Count;
            for (int i = 0; i < count; i++) {
                UpdateManager manager = _run[i];
                if (null == manager) {
                    // If the manager is null.
                    // Delete it and the index--.
                    _run.RemoveAt (i--);
                    continue;
                }
                manager.FixedUpdate (fdt);
            }
        }

        private void LateUpdate () {
            float dt = Time.deltaTime;
            int count = _run.Count;
            for (int i = 0; i < count; i++) {
                UpdateManager manager = _run[i];
                if (null == manager) {
                    // If the manager is null.
                    // Delete it and the index--.
                    _run.RemoveAt (i--);
                    continue;
                }
                manager.LateUpdate (dt);
            }
        }

        /// <summary>
        /// 創建UpdateManager.
        /// </summary>
        /// <param name="name">UpdateManager的名稱</param>
        /// <param name="autoRun">創建後是否自動開始執行</param>
        public UpdateManager Create (string name, bool autoRun = true) {
            UpdateManager manager = Get (name);
            // Create a new UpdateManager when not exist.
            if (null == manager) {
                manager = new UpdateManager (name);
                _managers.Add (manager);
                if (autoRun)
                    _run.Add (manager);
            }
            
            return manager;
        }

        /// <summary>
        /// 新增Updatable
        /// </summary>
        public void AddUpdatable (string name, IUpdatable updatable) {
            // Check UpdateManager is exist?
            // If not exist, create a new one.
            UpdateManager manager = Create (name);
            manager.Add (updatable);
        }

        /// <summary>
        /// 新增FixedUpdatable
        /// </summary>
        public void AddFixedUpdatable (string name, IFixedUpdatable fixedUpdatable) {
            // Check UpdateManager is exist?
            // If not exist, create a new one.
            UpdateManager manager = Create (name);
            manager.Add (fixedUpdatable);
        }

        /// <summary>
        /// 新增LateUpdatable
        /// </summary>
        public void AddLateUpdatable (string name, ILateUpdatable lateUpdatable) {
            // Check UpdateManager is exist?
            // If not exist, create a new one.
            UpdateManager manager = Create (name);
            manager.Add (lateUpdatable);
        }

        /// <summary>
        /// 刪除Updatable
        /// </summary>
        public void RemoveUpdatable (string name, IUpdatable updatable) {
            UpdateManager manager = Get (name);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            manager.Remove (updatable);
        }

        /// <summary>
        /// 刪除FixedUpdatable
        /// </summary>
        public void RemoveFixedUpdatable (string name, IFixedUpdatable fixedUpdatable) {
            UpdateManager manager = Get (name);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            manager.Remove (fixedUpdatable);
        }

        /// <summary>
        /// 刪除LateUpdatable
        /// </summary>
        public void RemoveLateUpdatable (string name, ILateUpdatable lateUpdatable) {
            UpdateManager manager = Get (name);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            manager.Remove (lateUpdatable);
        }        

        /// <summary>
        /// 設置Update執不執行
        /// </summary>
        public void SetActive (string name, bool isRun) {
            UpdateManager manager = Get (name);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            if (isRun)
                _run.Add (manager);
            else
                _run.Remove (manager);
        }

        /// <summary>
        /// 設置UpdateManager名稱
        /// </summary>
        public void Set (string name, string newName) {
            Get (name).Name = newName;
        }

        /// <summary>
        /// 根據名稱獲取UpdateManager
        /// </summary>
        public UpdateManager Get (string name) {
            int count = _managers.Count;
            for (int i = 0; i < count; i++) {
                UpdateManager manager = _managers[i];
                if (manager.Name == name) {
                    return manager;
                }
            }
            return null;
        }

        /// <summary>
        /// 刪除所有UpdateManager
        /// </summary>
        public void DeleteAll () {
            _managers.Clear ();
            _run.Clear ();
        }

        /// <summary>
        /// 刪除特定的UpdateManager
        /// </summary>
        public void Delete (string name) {
            UpdateManager manager = Get (name);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            _managers.Remove (manager);
            _run.Remove (manager);
        }

        /// <summary>
        /// 清除UpdateManager裏的所有子項
        /// </summary>
        public void ClearAll () {
            int count = _managers.Count;
            for (int i = 0; i < count; i++) {
                _managers[i].ClearAll ();
            }
        }

        /// <summary>
        /// 清除特定的UpdateManager裏的所有子項
        /// </summary>
        public void Clear (string name) {
            Get (name).ClearAll ();
        }





        #region Default

        private bool _isCreatedDefault = false;
        public const string DefaultName = "Default";
        
        /// <summary>
        /// 創建默認的UpdateManager
        /// </summary>
        public UpdateManager Create () {
            if (!_isCreatedDefault) {
                _isCreatedDefault = true;
                return Create (DefaultName);
            } else {
                return Get (DefaultName);
            }
        }

        /// <summary>
        /// 新增到默認的UpdateManager
        /// </summary>
        public void AddUpdatable (IUpdatable updatable) {
            UpdateManager manager = Create ();
            manager.Add (updatable);
        }


        /// <summary>
        /// 新增到默認的UpdateManager
        /// </summary>
        public void AddFixedUpdatable (IFixedUpdatable fixedUpdatable) {
            UpdateManager manager = Create ();
            manager.Add (fixedUpdatable);
        }

        /// <summary>
        /// 新增到默認的UpdateManager
        /// </summary>
        public void AddLateUpdatable (ILateUpdatable lateUpdatable) {
            UpdateManager manager = Create ();
            manager.Add (lateUpdatable);
        }

        /// <summary>
        /// 刪除Updatable
        /// </summary>
        public void RemoveUpdatable (IUpdatable updatable) {
            UpdateManager manager = Get (DefaultName);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + DefaultName + "\" UpdateManager.");
                return;
            }
            manager.Remove (updatable);
        }

        /// <summary>
        /// 刪除FixedUpdatable
        /// </summary>
        public void RemoveFixedUpdatable (IFixedUpdatable fixedUpdatable) {
            UpdateManager manager = Get (DefaultName);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + DefaultName + "\" UpdateManager.");
                return;
            }
            manager.Remove (fixedUpdatable);
        }

        /// <summary>
        /// 刪除LateUpdatable
        /// </summary>
        public void RemoveLateUpdatable (ILateUpdatable lateUpdatable) {
            UpdateManager manager = Get (DefaultName);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + DefaultName + "\" UpdateManager.");
                return;
            }
            manager.Remove (lateUpdatable);
        } 

        /// <summary>
        /// 獲取默認的UpdateManager
        /// </summary>
        public UpdateManager Get () {
            return Get (DefaultName);
        }

        public void SetActive (bool isRun) {
            UpdateManager manager = Get (DefaultName);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            if (isRun)
                _run.Add (manager);
            else
                _run.Remove (manager);
        }

        /// <summary>
        /// 刪除默認的UpdateManager
        /// </summary>
        public void Delete () {
            _isCreatedDefault = false;
            UpdateManager manager = Get (name);
            if (null == manager) {
                Debug.LogWarning ("Can't find \"" + name + "\" UpdateManager.");
                return;
            }
            _managers.Remove (manager);
            _run.Remove (manager);
        }

        /// <summary>
        /// 清除默認的UpdateManager的子項
        /// </summary>
        public void Clear () {
            Get (DefaultName).ClearAll ();
        }

        #endregion

    }

}