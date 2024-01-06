using Kuma;
using Kuma.Utils.UpdateManager;

using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IUpdatable {
    [Header ("外觀")] [Tooltip ("0:下 1:上 2:右 3:左")] [SerializeField]
    private GameObject[] m_HoldItemRoots; // 0: Down 1: Up 2: Right 3: Left
    [SerializeField] private SpriteRenderer[] m_HoldItemSpriteRenderers;

    [Header ("組件")] [SerializeField] private Interactor m_InteractiveMachine;

    [Header ("屬性")] [SerializeField] private float m_MoveSpeed = 5.0f;

    private Direction m_FaceTo = Direction.Lower;
    public Direction FaceTo {
        get { return m_FaceTo; }
        protected set {
            if (m_FaceTo != value) {
                Direction temp = m_FaceTo;
                m_FaceTo = value;
                OnMoveDirectionChanged (temp, value);
            }
        }
    }

    public Vector2 VectorDirection {
        get {
            Vector2 dir = Vector2.down;
            switch (FaceTo) {
                case Direction.Left:
                    dir = Vector2.left;

                    break;
                case Direction.Right:
                    dir = Vector2.right;

                    break;
                case Direction.Upper:
                    dir = Vector2.up;

                    break;
                case Direction.Lower:
                    dir = Vector2.down;

                    break;
            }
            return dir;
        }
    }
    
    protected Animator m_Animator;
    protected Rigidbody2D m_Rig;
    
#region Entity狀態

    public override void Init () {
        base.Init ();

        m_Rig = GetComponent<Rigidbody2D> ();
        m_Animator = GetComponent<Animator> ();
        
        InventoryManager.Instance.onEquipChanged += OnEquipChanged;
        UpdateDispatcher.Instance.AddUpdatable (this);

        PlayDefaultAnimation ();
        SetEquipActive (0);
    }

    public void GameClose () {
        UpdateDispatcher.Instance.RemoveUpdatable (this);
    }

    public override void Load () {
        base.Load ();
        
        ItemData itemData = DataManager.GetItemData (InventoryManager.Instance.CurrentEquip);
        if (itemData != null)
            ChangeEquippingItem (itemData.Icon);
        else
            ChangeEquippingItem (null);
    }
    
#endregion

    public void CallUpdate (float deltaTime) {
        KeyPressHandler ();
        KeyHandler ();
        
        if (GameManager.Instance.GameStatus != GameStatus.Gaming) {
            if (IsRunning) {
                StopMove ();
            }

            return;
        }

        // Movement
        ResetZByY ();
        Move ();
        
        
        // interact
        if (CheckInteractable ()) { }
    }

#region 移動

    private List<KeyCode> m_KeyCodes = new List<KeyCode> ();
    public float MoveSpeed => m_MoveSpeed;
    public bool IsRunning { get; private set; }

    private void KeyHandler () {
        if (!Input.GetKey (KeyCode.D)) {
            m_KeyCodes.Remove (KeyCode.D);
        }

        if (!Input.GetKey (KeyCode.A)) {
            m_KeyCodes.Remove (KeyCode.A);
        }

        if (!Input.GetKey (KeyCode.W)) {
            m_KeyCodes.Remove (KeyCode.W);
        }

        if (!Input.GetKey (KeyCode.S)) {
            m_KeyCodes.Remove (KeyCode.S);
        }
    }
    
    private void KeyPressHandler () {
        if (Input.GetKeyDown (KeyCode.D)) {
            m_KeyCodes.Add (KeyCode.D);
        }

        if (Input.GetKeyDown (KeyCode.A)) {
            m_KeyCodes.Add (KeyCode.A);
        }

        if (Input.GetKeyDown (KeyCode.W)) {
            m_KeyCodes.Add (KeyCode.W);
        }

        if (Input.GetKeyDown (KeyCode.S)) {
            m_KeyCodes.Add (KeyCode.S);
        }
    }

    private void KeyUpHandler () {
        if (Input.GetKeyUp (KeyCode.D)) {
            m_KeyCodes.Remove (KeyCode.D);
        }

        if (Input.GetKeyUp (KeyCode.A)) {
            m_KeyCodes.Remove (KeyCode.A);
        }

        if (Input.GetKeyUp (KeyCode.W)) {
            m_KeyCodes.Remove (KeyCode.W);
        }

        if (Input.GetKeyUp (KeyCode.S)) {
            m_KeyCodes.Remove (KeyCode.S);
        }
    }

    private void Move () {
        int count = m_KeyCodes.Count;
        if (count <= 0) {
            if (IsRunning) {
                StopMove ();
            }

            return;
        }

        KeyCode keyCode = m_KeyCodes[count - 1];
        Vector2 movement = Vector2.zero;
        Direction faceTo = Direction.Left;
        switch (keyCode) {
            case KeyCode.A:
                movement = Vector2.left * (m_MoveSpeed * Time.deltaTime);
                faceTo = Direction.Left;

                break;
            case KeyCode.D:
                movement = Vector2.right * (m_MoveSpeed * Time.deltaTime);
                faceTo = Direction.Right;

                break;
            case KeyCode.W:
                movement = Vector2.up * (m_MoveSpeed * Time.deltaTime);
                faceTo = Direction.Upper;

                break;
            case KeyCode.S:
                movement = Vector2.down * (m_MoveSpeed * Time.deltaTime);
                faceTo = Direction.Lower;

                break;
        }

        // 如果前一幀不是在運動狀態, 則執行開始運動的代碼
        if (!IsRunning) {
            FaceTo = faceTo;
            StartMove ();
        } else {
            // 如果前一幀處於運動狀態, 但是運動方向也不同, 則執行方向改變的代碼
            if (faceTo != FaceTo) {
                OnMoveDirectionChanged (FaceTo, faceTo);
            }
        }

        CacheTf.position = CacheTf.position + (Vector3)movement;
        FaceTo = faceTo;

        AudioManager.Instance.PlayMove();
    }

    //
    // 移動方向改變
    //
    private void OnMoveDirectionChanged (Direction oldDir, Direction newDir) {
        switch (newDir) {
            case Direction.Left:
                SetEquipActive (3);
                PlayAnimation (WalkLeftAnimationName);
                break;
            case Direction.Right:
                SetEquipActive (2);
                PlayAnimation (WalkRightAnimationName);
                break;
            case Direction.Upper:
                SetEquipActive (1);
                PlayAnimation (WalkUpAnimationName);
                break;
            case Direction.Lower:
                SetEquipActive (0);
                PlayAnimation (WalkDownAnimationName);
                break;
        }
    }

    private void StopMove () {
        IsRunning = false;
        
        switch (FaceTo) {
            case Direction.Left:
                PlayAnimation (IdleLeftAnimationName);

                break;
            case Direction.Right:
                PlayAnimation (IdleRightAnimationName);

                break;
            case Direction.Upper:
                PlayAnimation (IdleUpAnimationName);

                break;
            case Direction.Lower:
                PlayAnimation (IdleDownAnimationName);

                break;
        }
    }

    public virtual void StartMove () {
        IsRunning = true;
        
        switch (FaceTo) {
            case Direction.Left:
                PlayAnimation (WalkLeftAnimationName);

                break;
            case Direction.Right:
                PlayAnimation (WalkRightAnimationName);

                break;
            case Direction.Upper:
                PlayAnimation (WalkUpAnimationName);

                break;
            case Direction.Lower:
                PlayAnimation (WalkDownAnimationName);

                break;
        }
    }

#endregion

#region 外觀

    private void ChangeEquippingItem (Sprite sprite) {
        for (int i = 0; i < m_HoldItemSpriteRenderers.Length; ++i) {
            m_HoldItemSpriteRenderers[i].sprite = sprite;
        }
    }

    private void SetEquipActive (int showIndex) {
        for (int i = 0; i < m_HoldItemRoots.Length; ++i) {
            m_HoldItemRoots[i].SetActive (false);
        }

        m_HoldItemRoots[showIndex].SetActive (true);
    }

    private void OnEquipChanged (int itemId, int slotId) {
        if (itemId < 0) {
            ChangeEquippingItem (null);

            return;
        }

        ItemData itemData = DataManager.GetItemData (itemId);
        ChangeEquippingItem (itemData.Icon);
    }

#endregion

#region 動畫

    private string IdleDownAnimationName => "Idle_Down";
    private string IdleUpAnimationName => "Idle_Up";
    private string IdleLeftAnimationName => "Idle_Left";
    private string IdleRightAnimationName => "Idle_Right";

    private string WalkDownAnimationName => "Walk_Down";
    private string WalkUpAnimationName => "Walk_Up";
    private string WalkLeftAnimationName => "Walk_Left";
    private string WalkRightAnimationName => "Walk_Right";
    
    protected void PlayDefaultAnimation () {
        PlayAnimation (IdleDownAnimationName);
    }

    protected void PlayAnimation (string name) {
        m_Animator.SetTrigger (name);
    }

#endregion

#region 交互

    public void Interact () {
        if (m_InteractiveMachine.HasInteractable) {
            Interactor.InteractableElement element = m_InteractiveMachine.GetNearest (CacheTf.position);
            
            if (element.Obj.Interact ()) {

            }
        }
    }

    private bool CheckInteractable () {
        Vector2 dir = Vector2.right;
        switch (FaceTo) {
            case Direction.Left:
                dir = Vector2.left;

                break;
            case Direction.Right:
                dir = Vector2.right;

                break;
            case Direction.Upper:
                dir = Vector2.up;

                break;
            case Direction.Lower:
                dir = Vector2.down;

                break;
        }

        if (m_InteractiveMachine.Check (dir)) {
            Interactor.InteractableElement element = m_InteractiveMachine.GetNearest (CacheTf.position);
            ModuleManager.Instance.FindModule<GameModule> ().NearstElement = element;

            return true;
        } else {
            ModuleManager.Instance.FindModule<GameModule> ().NearstElement = null;
        }

        return false;
    }

#endregion

}
