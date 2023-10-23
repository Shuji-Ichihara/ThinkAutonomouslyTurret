using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region Refarences
    // 大砲の Prefab
    public GameObject Cannon => _cannon;
    [SerializeField]
    private GameObject _cannon = null;
    // Target の親オブジェクト
    [SerializeField]
    private TargetPool _targetPool = null;
    private Transform _targetSpawnZone = null;
    #endregion
    #region Time
    // 制限時間
    [SerializeField]
    private float _setGameTime = 60.0f;
    private float _gameTime = 0.0f;
    public float GameTime => _gameTime;
    // Target のスポーンする間隔
    [SerializeField]
    private float _spawningIntervalTime = 5.0f;
    private float _countInterval = 0.0f;
    #endregion
    #region Score
    // スコア
    private int _gameScore = 0;
    public int GameScore => _gameScore;
    #endregion
    // 的が一度にスポーンする個数
    [SerializeField, Range(0, 10)]
    private int _spawningNumberOfTargets = 5;

    new private void Awake()
    {
        InitGame();
    }

    // Start is called before the first frame update
    async void Start()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        _gameTime = _setGameTime;
        _countInterval = 0.0f;
        _targetSpawnZone = GameObject.Find("TargetSpawnZone").GetComponent<Transform>();
        for (int i = 0; i < _spawningNumberOfTargets; i++)
        {
            SpawnTarget();
        }
        await CountGameTime(cts);
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
    /// 制限時間のカウント
    /// </summary>
    private async UniTask CountGameTime(CancellationTokenSource cts = default)
    {
        while (_gameTime > 0.0f)
        {
            try
            {
                await UniTask.Yield(cts.Token);
                _gameTime -= Time.deltaTime;
                _countInterval += Time.deltaTime;
                // 的を配置
                if (_countInterval > _spawningIntervalTime)
                {
                    for (int i = 0; i < _spawningNumberOfTargets; i++)
                    {
                        SpawnTarget();
                    }
                    _countInterval = 0.0f;
                }
            }
            catch (MissingReferenceException)
            {
                cts.Cancel();
                continue;
            }
        }
        // 半端な値になるため、ループを抜けたら 0 を代入
        _gameTime = 0.0f;
        cts.Cancel();
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
    /// <param name="number">配置する的の種類</param>
    /// <returns>的を配置する座標</returns>
    public Vector3 CalculateSpawnPosition(int number)
    {
        bool isSapwnX = false, isSpawnZ = false;
        float spawnX = 0.0f, spawnY = 0.0f, spawnZ = 0.0f;
        var target = _targetPool.TargetType[number].GetComponentInChildren<Target>();
        Vector3 targetScale = target.gameObject.transform.localScale;
        // 指定範囲内にランダムに配置
        // X 軸と Z 軸は Cannon と重ならないようにする
        while (isSapwnX == false)
        {
            float dummyX = Random.Range(-_targetSpawnZone.localScale.x / 2, _targetSpawnZone.localScale.x / 2);
            if (dummyX > Cannon.transform.position.x + 10.0f || dummyX < Cannon.transform.position.x - 10.0f)
            {
                spawnX = dummyX;
                isSapwnX = true;
            }
        }
        while (isSpawnZ == false)
        {
            float dummyZ = Random.Range(-_targetSpawnZone.localScale.z / 2, _targetSpawnZone.localScale.z / 2);
            if (dummyZ > Cannon.transform.position.z + 10.0f || dummyZ < Cannon.transform.position.z - 10.0f)
            {
                spawnZ = dummyZ;
                isSpawnZ = true;
            }
        }
        spawnY = Random.Range(targetScale.z / 2, (_targetSpawnZone.localScale.y / 2) - (targetScale.z / 2));
        // 的が地面に埋め込まれないように配置する高さを調整
        if (spawnY < targetScale.x)
        {
            spawnY = targetScale.x;
        }
        return new Vector3(spawnX, spawnY, spawnZ);
    }
}
