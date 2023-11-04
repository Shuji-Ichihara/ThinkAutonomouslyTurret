using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class TitleUIManager : SingletonMonoBehaviour<TitleUIManager>
{
    // タイトルのテキストコンポーネント
    [SerializeField]
    private TextMeshProUGUI _titleTextComponent = null;
    // タイトルに表示する文字
    [SerializeField]
    private string _titleText = "自律思考型固定砲台";
    // ゲームのスタートを案内するテキストコンポーネント
    [SerializeField]
    private TextMeshProUGUI _gameStartTextComponent = null;
    // ゲームのスタートを案内するテキストに表示する文字
    [SerializeField]
    private string _gameStartText = "Spece を押したらスタート";
    // 降下アニメーションの再生時間
    [SerializeField, Range(0.0f, 270.0f)]
    private float _movedownAnimationTime = 90.0f;
    // 点滅アニメーションの再生時間
    [SerializeField]
    private float _blinkAnimationTime = 5.0f;

    // タイトルテキストのフォントサイズ
    private readonly float _titleFontSize = 180.0f;
    // ゲームスタートの案内テキストのフォントサイズ
    private readonly float _gameStartFontSize = 60.0f;

    CancellationTokenSource _titleAnimationToken = new CancellationTokenSource();
    CancellationTokenSource _gameStartAnimationToken = new CancellationTokenSource();

    // Start is called before the first frame update
    private async void Start()
    {
        SetUpText();
        await MoveDownTextAnimation(_movedownAnimationTime, _titleAnimationToken);
        TitleSceneManager.Instance.IsActivatedChangeScene();
        _gameStartTextComponent.gameObject.SetActive(true);
        await BlinkTextAnimation(_blinkAnimationTime, _gameStartAnimationToken);
    }

    /// <summary>
    /// テキストコンポーネントの取得や設定を行う
    /// </summary>
    private void SetUpText()
    {
        if (_titleTextComponent == null)
        {
            _titleTextComponent = GameObject.Find("GameTitle").GetComponent<TextMeshProUGUI>();
        }
        _titleTextComponent.fontSize = _titleFontSize;
        _titleTextComponent.rectTransform.anchoredPosition = Vector2.up * _titleTextComponent.rectTransform.rect.height;
        _titleTextComponent.text = _titleText;
        if (_gameStartTextComponent == null)
        {
            _gameStartTextComponent = GameObject.Find("GameStart").GetComponent<TextMeshProUGUI>();
        }
        _gameStartTextComponent.fontSize = _gameStartFontSize;
        _gameStartTextComponent.alignment = TextAlignmentOptions.Center;
        _gameStartTextComponent.text = _gameStartText;
        _gameStartTextComponent.gameObject.SetActive(false);
    }

    /// <summary>
    /// タイトルテキストの降下アニメーション
    /// </summary>
    /// <param name="moveDownAnimationTime"></param>
    /// <param name="cts"></param>
    /// <returns></returns>
    private async UniTask MoveDownTextAnimation(float moveDownAnimationTime, CancellationTokenSource cts = default)
    {
        // テキストの最終的な Y 座標
        RectTransform rectTransform = _titleTextComponent.rectTransform;
        float titleTextYPosition = -90.0f;
        while (rectTransform.anchoredPosition.y > titleTextYPosition)
        {
            try
            {
                rectTransform.anchoredPosition += Vector2.down * (Time.deltaTime * moveDownAnimationTime);
                await UniTask.Yield(cts.Token);
            }
            catch (Exception)
            {
                break;
            }
        }
        if (rectTransform.anchoredPosition.y < titleTextYPosition)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, titleTextYPosition);
        }
    }

    private async UniTask BlinkTextAnimation(float blinkAnimationDuration, CancellationTokenSource cts = default)
    {
        Color textColor = _gameStartTextComponent.color;
        float alpha = 0.0f;
        // while ループを止める変数、 テキストが表示されているかの変数
        bool isStoped = false, isPreviewedText = false;
        while (isStoped == false)
        {
            try
            {
                // true の場合は透明、false の場合は不透明に近づける
                switch (isPreviewedText)
                {
                    case true:
                        alpha -= Time.deltaTime / blinkAnimationDuration * 2;
                        _gameStartTextComponent.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
                        if (alpha <= 0.0f)
                        {
                            alpha = 0.0f;
                            isPreviewedText = false;
                        }
                        break;
                    case false:
                        alpha += Time.deltaTime / blinkAnimationDuration * 2;
                        _gameStartTextComponent.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
                        if (alpha >= 1.0f)
                        {
                            alpha = 1.0f;
                            isPreviewedText = true;
                        }
                        break;
                }
                await UniTask.Yield(cts.Token);
            }
            catch (Exception)
            {
                isStoped = true;
                break;
            }
        }

    }
}
