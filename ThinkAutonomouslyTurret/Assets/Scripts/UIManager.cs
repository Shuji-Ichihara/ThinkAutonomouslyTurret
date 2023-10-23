using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // ゲームシーンのキャンバス
    [SerializeField]
    private Canvas _gameUICanvas = null;
    // スコアを表示するテキスト
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;
    // 制限時間を表示するテキスト
    [SerializeField]
    private TextMeshProUGUI _timeText = null;

    private Target _target = null; 

    // Start is called before the first frame update
    void Start()
    {
        _gameUICanvas.worldCamera = Camera.main;
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
        _scoreText.alignment = TextAlignmentOptions.Right;
    }

    /// <summary>
    /// ダメージ、スコアアップテキストを表示する
    /// </summary>
    /// <param name="obj"></param>
    public void PopUpText(GameObject obj)
    {

    }

}
