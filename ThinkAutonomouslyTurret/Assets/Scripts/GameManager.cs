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
    [SerializeField]
    private TargetPool _targetPool = null;

    private Transform _targetSpawnZone = null;

    new private void Awake()
    {
        InitGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameTime = _setGameTime;
        //_targetPool = GameObject.Find("TargetPool").GetComponent<TargetPool>();
        _targetSpawnZone = GameObject.Find("TargetSpawnZone").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownGameTime();
        SpawnTarget();
    }

    /// <summary>
    /// ゲームに必要なオブジェクト群を生成
    /// </summary>
    private void InitGame()
    {
        _cannon = Instantiate(_cannon, Vector3.up / 2, Quaternion.identity);
        BulletPool bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        bulletPool.InitObjectPool();
        _targetPool.InitObjectPool();
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

    /// <summary>
    /// 的をシーン上に配置
    /// </summary>
    private void SpawnTarget()
    {
        int randomNumber = Random.Range(0, _targetPool.TargetType.Length);
        Vector3 spawnPosition = CalculateSpawnPosition(randomNumber);
        _targetPool.ActivateObject(spawnPosition, randomNumber);
    }

    /// <summary>
    /// スポーンする座標を計算
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateSpawnPosition(int number)
    {
        Vector3 targetScale = _targetPool.TargetType[number].transform.position;
        float x = Random.Range(-_targetSpawnZone.localScale.x / 2, _targetSpawnZone.localScale.x / 2);
        float y = Random.Range(targetScale.z / 2, (_targetSpawnZone.localScale.y / 2) - (targetScale.z / 2));
        float z = Random.Range(-_targetSpawnZone.localScale.z / 2, _targetSpawnZone.localScale.z / 2);
        return new Vector3(x, y, z);
    }
}
