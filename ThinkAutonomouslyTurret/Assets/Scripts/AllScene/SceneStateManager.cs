using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンステート
/// </summary>
public enum SceneState
{
    TitleScene,
    GameScene,
    ResultScene,
}

public class SceneStateManager : SingletonMonoBehaviour<SceneStateManager>
{
    public SceneState SceneStatus { get; private set; }

    // Start is called before the first frame update
    new private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneStatus = SceneState.TitleScene;
    }

    /*
    private void Update()
    {
        QuitApplication();
    }
    */

    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <param name="sceneState">遷移先のシーンステート</param>
    public void ChangeScene(SceneState sceneState)
    {
        SceneManager.LoadScene((int)sceneState);
        SceneStatus = sceneState;
    }

    /// <summary>
    /// アプリケーション終了
    /// </summary>
    private void QuitApplication()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
