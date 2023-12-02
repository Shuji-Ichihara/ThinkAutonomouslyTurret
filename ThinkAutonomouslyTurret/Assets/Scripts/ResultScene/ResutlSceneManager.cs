using UnityEngine;

public class ResutlSceneManager : SingletonMonoBehaviour<ResutlSceneManager>
{
    // シーン遷移が可能になるフラグ
    private bool _isActivatedChangeScene = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isActivatedChangeScene == true)
        {
            SceneStateManager.Instance.ChangeScene(SceneState.TitleScene);
            _isActivatedChangeScene = false;
        }
    }

    public void IsActivatedChageScene()
    {
        _isActivatedChangeScene = true;
    }
}
