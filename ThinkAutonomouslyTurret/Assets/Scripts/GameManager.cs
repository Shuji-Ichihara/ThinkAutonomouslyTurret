using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // 大砲の Prefab
    [SerializeField]
    private GameObject _canon = null;
    // 制限時間
    [SerializeField]
    private float _setGameTime = 60.0f;
    private float _gameTime = 0.0f;
    public float GameTime => _gameTime;

    new private void Awake()
    {
        InitGame();
        BulletPool bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        bulletPool.InitPoolBullet();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameTime = _setGameTime;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownGameTime();
    }

    private void InitGame()
    {
        _canon = Instantiate(_canon, Vector3.up, Quaternion.identity);
    }

    private void CountDownGameTime()
    {
        _gameTime -= Time.deltaTime;
    }
}
