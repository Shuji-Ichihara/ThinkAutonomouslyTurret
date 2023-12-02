using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class ResultUIManager : SingletonMonoBehaviour<ResultUIManager>
{
    #region Refarences
    [SerializeField]
    private TextMeshProUGUI _playerText = null;
    // スコアを表示するテキスト
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;
    // スタートへ戻る案内を表示するテキスト
    [SerializeField]
    private TextMeshProUGUI _returnToStartText = null;
    #endregion
    #region Messages
    // _playerText に表示するメッセージ
    [SerializeField]
    private string _playerMessage = "Player";
    // _returnToStartText に表示するメッセージ
    [SerializeField]
    private string _returnToStartMessage = "ReturnToStart";
    #endregion
    #region DurationTimeSettings
    // スコアを表示するまで待機する時間
    [SerializeField]
    private float _firstDurationTime = 2.0f;
    // タイトルの案内テキストを表示するまで待機する時間
    [SerializeField]
    private float _secondDurationTime = 2.0f;
    #endregion
    #region TextSettings
    [SerializeField]
    private float _playerTextFontSize = 80.0f;
    [SerializeField]
    private float _scoreTextFontSize = 120.0f;
    [SerializeField]
    private float _returnToStartTextFontSize = 80.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetUpText();
        CallPreviewText();
    }

    /// <summary>
    /// テキストコンポーネントの取得、設定を行う
    /// </summary>
    private void SetUpText()
    {
        // null チェック
        if (_playerText == null)
            _playerText = GameObject.Find("Player").GetComponent<TextMeshProUGUI>();
        if (_scoreText == null) 
            _scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>(); 
        if (_returnToStartText == null) 
            _returnToStartText = GameObject.Find("ReturnToStart").GetComponent<TextMeshProUGUI>(); 
        // 表示するメッセージを代入
        _playerText.text = _playerMessage;
        _scoreText.text = GameSceneManager.GameScore.ToString() + " 点";
        _returnToStartText.text = _returnToStartMessage;
        // フォントサイズの設定
        _playerText.fontSize = _playerTextFontSize;
        _scoreText.fontSize = _scoreTextFontSize;
        _returnToStartText.fontSize = _returnToStartTextFontSize;
        // 非アクティブ化
        _playerText.gameObject.SetActive(false);
        _scoreText.gameObject.SetActive(false);
        _returnToStartText.gameObject.SetActive(false);
    }

    /// <summary>
    /// テキストを順番に表示する
    /// </summary>
    /// <param name="cts"></param>
    /// <returns></returns>
    private async UniTask DisplayTextInOrder(CancellationTokenSource cts = default)
    {
        _playerText.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(_firstDurationTime, cancellationToken: cts.Token);
        AudioManager.Instance.PlaySE(SEType.PreviewText);
        _scoreText.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(_secondDurationTime, cancellationToken: cts.Token);
        AudioManager.Instance.PlayBGM(BGMType.ResultBGM);
        _returnToStartText.gameObject.SetActive(true);
        ResutlSceneManager.Instance.IsActivatedChageScene();
    }

    /// <summary>
    /// DisplayTextInOrder を呼び出す関数
    /// </summary>
    private void CallPreviewText()
    {
        CancellationTokenSource previewTextToken = new CancellationTokenSource();
        DisplayTextInOrder(previewTextToken).Forget();
    }
}
