using UnityEngine;

public class TitleSceneManager : SingletonMonoBehaviour<TitleSceneManager>
{
    // シーン遷移が可能になるフラグ
    private bool _isChangedScene = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isChangedScene == true)
        {
            SceneStateManager.Instance.ChangeScene(SceneState.GameScene);
            _isChangedScene = false;
        }
    }

    /// <summary>
    /// シーン変更が可能なフラグを有効にする
    /// </summary>
    public void IsActivatedChangeScene()
    {
        _isChangedScene = true;
    }
}
