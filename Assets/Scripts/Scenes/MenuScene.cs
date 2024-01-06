using UnityEngine;

public class MenuScene : BaseScene {
    public override SceneType GetSceneType () {
        return SceneType.Menu;
    }

    public override void Open () {
        base.Open ();
        
        UIManager.OpenUI (UIType.Menu);

        AudioManager.Instance.PlayBGM ("Menu");
    }
    
}