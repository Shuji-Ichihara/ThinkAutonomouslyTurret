using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // 大砲の Prefab
    [SerializeField]
    private GameObject _cannon = null;
    // 制限時間
    [SerializeField]
    private float _setGameTime = 60.0f;
    private static float _gameTime = 0.0f;
    public static float GameTime => _gameTime;
    // スコア
    private int _gameScore = 0;
    public int GameScore => _gameScore;

    private TargetPool _targetPool = null;

    new private void Awake()
    {
        InitGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameTime = _setGameTime;
        _targetPool = GameObject.Find("TargetPool").GetComponent<TargetPool>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownGameTime();
    }

    /// <summary>
    /// ゲームに必要なオブジェクト群を生成
    /// </summary>
    private void InitGame()
    {
        _cannon = Instantiate(_cannon, Vector3.up / 2, Quaternion.identity);
        BulletPool bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        bulletPool.InitPoolBullet();
    }

    /// <summary>
    /// 制限時間のカウントダウン
    /// </summary>
    private void CountDownGameTime()
    {
        _gameTime -= Time.deltaTime;
    }

    /// <summary>
    /// スコア上昇
    /// </summary>
    /// <param name="score">上昇するスコアの値</param>
    public void CountUpScore(int score)
    {
        _gameScore += score;
    }
    
    private void SpawnTarget()
    {

    }
}
