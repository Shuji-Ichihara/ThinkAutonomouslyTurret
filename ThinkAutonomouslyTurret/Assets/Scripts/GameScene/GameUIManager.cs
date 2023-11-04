using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
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
    // ポップアップするテキスト(的へのダメージ)
    [SerializeField]
    private TextMeshProUGUI _popUpDamageText = null;
    // ポップアップするテキスト(スコア加算)
    [SerializeField]
    private TextMeshProUGUI _popUpScoreText = null;
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
        _timeText.text = string.Format("{0:#}", GameSceneManager.Instance.GameTime);
        _scoreText.text = "Score : " + GameSceneManager.Instance.GameScore.ToString();
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
        _popUpDamageText.color = Color.black;
        _popUpScoreText.color = Color.black;
        _popUpScoreText.rectTransform.pivot = Vector2.one;
    }

    /// <summary>
    /// ダメージテキストを表示する
    /// </summary>
    /// <param name="Damage">的へのダメージ</param>
    private async UniTask PopUpDamageText(int Damage = 0)
    {
        try
        {
            _popUpDamageText.text = "-" + Damage.ToString();
            RectTransform canvasTransform = _gameUICanvas.GetComponent<RectTransform>();
            GameObject textComponent = Instantiate(_popUpDamageText.gameObject, _gameUICanvas.transform);
            RectTransform textComponentTransform = textComponent.GetComponent<RectTransform>();
            // スクリーン座標を UI 座標に変換
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTransform
                , textComponent.transform.position
                , null
                , out var vector2);
            // ゲームの仕様上、的は画面の中央に存在する為、やや右上にテキストを配置
            textComponentTransform.position += (Vector3.up + Vector3.right);
            await TextAnimation(textComponentTransform, token: this.GetCancellationTokenOnDestroy());
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// スコアアップテキストを表示する
    /// </summary>
    /// <param name="score">加算されるスコア</param>
    private async UniTask PopUpScoreText(int score = 0)
    {
        try
        {
            _popUpScoreText.text = "+" + score.ToString();
            GameObject textComponent = Instantiate(_popUpScoreText.gameObject, _gameUICanvas.transform);
            RectTransform textComponentTransform = textComponent.GetComponent<RectTransform>();
            // _popUpSocreText を _scoreText の下に配置
            textComponentTransform.anchoredPosition = Vector2.right * textComponentTransform.anchoredPosition + Vector2.down * _scoreText.rectTransform.rect.height;
            await TextAnimation(textComponentTransform, 2.0f, this.GetCancellationTokenOnDestroy());
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// ポップアップするテキストのアニメーション
    /// </summary>
    /// <param name="rectTransform">テキストコンポーネントの UI 座標</param>
    /// <param name="token">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask TextAnimation(RectTransform rectTransform, float animationLength = 1.0f, CancellationToken token = default)
    {
        float animationTime = 0.0f, half = 2.0f;
        // テキストの色情報を取得
        var text = rectTransform.gameObject.GetComponent<TextMeshProUGUI>();
        Color textColor = text.color;
        float alpha = 0.0f;
        // 疑似アニメーション再生
        while (animationTime < animationLength)
        {
            try
            {
                rectTransform.position += Vector3.up / half * Time.deltaTime;
                animationTime += Time.deltaTime;
                if (animationTime > animationLength / half)
                {
                    // 文字をフェードアウト
                    alpha = text.color.a;
                    alpha -= Time.deltaTime;
                    text.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
                }
                await UniTask.Yield(token);
            }
            catch (Exception)
            {
                break;
            }
        }
        Destroy(rectTransform.gameObject);
    }

    public void CallPopUpDamageText(int damage = 0)
    {
        PopUpDamageText(damage).Forget();
    }

    public void CallPopUpScoreText(int score = 0)
    {
        PopUpScoreText(score).Forget();
    }
}
