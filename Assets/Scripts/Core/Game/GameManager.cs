using UnityEngine;

using Kuma.Utils.Singleton;

public enum GameWinType {
    EndA,
    EndB,
}

public enum GameStatus {
    Loading,
    Initializing,
    Gaming,
    Pausing,
    Timeout,
    Recycle,
    End,
    Closing
}

public struct GameStatusChangedArgs {
    public GameStatus PrevStatus;
    public GameStatus CurStatus;
}

public delegate void GameStatusChangedEventHandler (GameStatusChangedArgs args);

public class GameManager : MonoSingleton<GameManager> {
    private int m_RecycleCount = 0;
    
    public event GameStatusChangedEventHandler GameStatusChangedEvent;

#region 游戲邏輯

    private GameStatus m_GameStatus;

    public GameStatus GameStatus {
        get { return m_GameStatus; }
        private set {
            if (m_GameStatus != value) {
                GameStatusChangedArgs args = new GameStatusChangedArgs ();
                args.PrevStatus = m_GameStatus;
                args.CurStatus = value;

                m_GameStatus = value;

                if (GameStatusChangedEvent != null)
                    GameStatusChangedEvent (args);
            }
        }
    }

    public void GameLoad (bool isNewGame) {
        GameStatus = GameStatus.Loading;
        
        InventoryManager.Instance.Load (isNewGame);
        ScenarioManager.Instance.Load (isNewGame);
        MapManager.Instance.Load (isNewGame);

        TimeManager.Instance.RegisterEvent (GameSettings.RecycleTime, OnGameTimeout);
    }

    public void GameInit () {
        GameStatus = GameStatus.Initializing;
        
        InventoryManager.Instance.Init ();
        MapManager.Instance.Init ();

        CameraController.Instance.StartFollow ();
    }

    public void GameStart () {
        AnalyticsManager.Track (AnalyticsEventKey.GameStart);

        ShowFirstGameCG ();
    }

    private void ShowFirstGameCG () {
        int id = 2;
        // 判斷觸不觸發劇情
        if (!ScenarioManager.Instance.CheckHasBeenShowById (id)) {
            ScenarioManager.Instance.AddHasBeenShowScenarioId (id);

            DialogUI ui =
                    (DialogUI)UIManager.OpenUI (UIType.DialogUI, ScenarioManager.Instance.GetScenarioDataById (id),
                                                false);
            ui.Exit += () => { GameStatus = GameStatus.Gaming; };
        } else {
            GameStatus = GameStatus.Gaming;
        }
    }
    
    public void GamePause () {
        GameStatus = GameStatus.Pausing;
    }

    public void GameContinue () {
        GameStatus = GameStatus.Gaming;
    }

    private void GameTimeout () {
        GameStatus = GameStatus.Timeout;

        MapManager.Instance.Timeout ();
    }

    // 輪迴
    private void GameRecycle () {
        GameStatus = GameStatus.Recycle;

        TimeManager.Instance.Reset ();
        MapManager.Instance.Recycle ();

        m_RecycleCount++;
    }

    public void GameWin (GameWinType type) {
        GameStatus = GameStatus.End;

        switch (type) {
            case GameWinType.EndA:
                AudioManager.Instance.PlayBGM("EndA");
                
                int id = 6;
                Scenario scenario = ScenarioManager.Instance.GetScenarioDataById (id);
                DialogUI ui = (DialogUI)UIManager.OpenUI (UIType.DialogUI, scenario, true);
                ui.FadeOutStart += () => {
                    GameClose ();
                    SceneManager.OpenScene (SceneType.Menu, false, true);
                };

                break;
            case GameWinType.EndB:
                AudioManager.Instance.PlayBGM("EndB");
                
                id = 10;
                scenario = ScenarioManager.Instance.GetScenarioDataById (id);
                ui = (DialogUI)UIManager.OpenUI (UIType.DialogUI, scenario, true);
                ui.FadeOutStart += () => {
                    GameClose ();
                    SceneManager.OpenScene (SceneType.Menu, false, true);
                };

                break;
        }

        AnalyticsManager.Track (AnalyticsEventKey.Clear, AnalyticsManager.GetDictionary ("end", type.ToString ()));
    }

    public void GameClose () {
        GameStatus = GameStatus.Closing;

        CameraController.Instance.CancelFollow ();
        CameraController.Instance.ResetPosition ();

        MapManager.Instance.GameClose ();

        UIManager.CloseUI (UIType.SelectedItemUI);
        UIManager.CloseUI (UIType.Inventory);
        UIManager.CloseUIs (UIType.TipUI);
    }

#endregion

    private void OnGameTimeout () {
        GameTimeout ();
        GameRecycle ();
        
        if (m_RecycleCount == 1) {
            int id = 4;
            if (!ScenarioManager.Instance.CheckHasBeenShowById (id)) {
                Scenario scenario = ScenarioManager.Instance.GetScenarioDataById(id);
                ScenarioManager.Instance.AddHasBeenShowScenarioId (id);
                DialogUI ui = (DialogUI)UIManager.OpenUI (UIType.DialogUI, scenario, false);
                ui.Exit += () => {
                    GameStart ();
                };
            } else {
                GameStart ();
            }
        } else {
            GameStart ();
        }
        
        UIManager.OpenUI (UIType.TipUI);
    }

}
