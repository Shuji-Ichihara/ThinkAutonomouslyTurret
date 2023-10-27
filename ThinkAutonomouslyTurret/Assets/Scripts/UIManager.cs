using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    #region Refarences
    [SerializeField]
    private Camera _uiCamera = null;
    // ゲームシーンのキャンバス
    [SerializeField]
    private Canvas _gameUICanvas = null;
    // スコアを表示するテキスト
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;
    // 制限時間を表示するテキスト
    [SerializeField]
    private TextMeshProUGUI _timeText = null;
    // ポップアップするテキスト
    [SerializeField]
    private TextMeshProUGUI _popUpText = null;
    #endregion

    //public Subject<GameObject> PopUpText = new Subject<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // UICamera の null チェック
        if (_gameUICanvas.worldCamera == null || _uiCamera == null)
        {
            _uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            _gameUICanvas.worldCamera = _uiCamera;
        }
        _uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
        // MainCamera に UICamera の情報を合成する
        var mainCameraData = Camera.main.GetUniversalAdditionalCameraData();
        mainCameraData.cameraStack.Add(_uiCamera);
        SetUpText();
    }

    // Update is called once per frame
    void Update()
    {
        _timeText.text = string.Format("{0:#}", GameManager.Instance.GameTime);
        _scoreText.text = "Score : " + GameManager.Instance.GameScore.ToString();
    }

    /// <summary>
    /// テキストの設定初期化
    /// </summary>
    private void SetUpText()
    {
        _timeText.fontSize = 120.0f;
        _scoreText.fontSize = 80.0f;
        _timeText.alignment = TextAlignmentOptions.Center;
        _scoreText.alignment = TextAlignmentOptions.Top;
        _popUpText.color = Color.black;
    }

    /// <summary>
    /// ダメージ、スコアアップテキストを表示する
    /// </summary>
    /// <param name="TargetObject">テキストを表示する座標にあるオブジェクト</param>
    public async void PopUpScoreText(GameObject TargetObject, int previewNumber = 0)
    {
        Target target = TargetObject.GetComponent<Target>();
        _popUpText.text = previewNumber.ToString();
        RectTransform canvasTransform = _gameUICanvas.GetComponent<RectTransform>();
        GameObject textComponent =Instantiate(_popUpText.gameObject, _gameUICanvas.transform);
        RectTransform textComponentTransform = textComponent.GetComponent<RectTransform>();
        // スクリーン座標を UI 座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasTransform
            , textComponent.transform.position
            , null
            , out var vector2);
        // 的の右上にテキストを配置
        textComponentTransform.position += (Vector3.up + Vector3.right) ;
        await AnimationText(textComponentTransform, this.GetCancellationTokenOnDestroy());
    }

    /// <summary>
    /// ポップアップするテキストのアニメーション
    /// </summary>
    /// <param name="rectTransform">テキストコンポーネントの UI 座標</param>
    /// <param name="token">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask AnimationText(RectTransform rectTransform, CancellationToken token = default)
    {
        float animationTime = 0.0f, endAnimationTime = 1.0f;
        while (animationTime < endAnimationTime )
        {
            rectTransform.position += Vector3.up / 2 * Time.deltaTime;
            animationTime += Time.deltaTime;
            await UniTask.Yield(token);
        }
        Destroy(rectTransform.gameObject);
    }
}
