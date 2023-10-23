using System.Collections.Generic;
using UnityEngine;

public class BulletPool : SingletonMonoBehaviour<BulletPool>, PoolFeature
{
    [SerializeField]
    private GameObject _bullet = null;

    private List<PoolingObject> _poolingObjects = new List<PoolingObject>();
    // 備蓄しておく Prefab の個数
    private const int _max = 100;

    /// <summary>
    /// 弾のオブジェクトプール生成
    /// </summary>
    public void InitObjectPool()
    {
        PoolingObject poolingObject;
        for (int i = 0; i < _max; i++)
        {
            poolingObject = Instantiate(_bullet, transform).GetComponent<PoolingObject>();
            _bullet.SetActive(false);
            _poolingObjects.Add(poolingObject);
        }
    }

    /// <summary>
    /// 弾オブジェクトを検索、アクティブ化
    /// </summary>
    /// <param name="spawnPosition">オブジェクトを生成する座標</param>
    /// <param name="number">ここでは使用しない</param>
    public void ActivateObject(Vector3 spawnPosition, int number = 0)
    {
        PoolingObject poolingObject;
        for (int i = 0; i < _poolingObjects.Count; i++)
        {
            poolingObject = _poolingObjects[i];
            if(false == poolingObject.gameObject.activeSelf)
            {
                poolingObject.InitObject(spawnPosition);
                return;
            }
        }
        // 万一、プール内のオブジェクトが不足した場合、新しく生成しプールに加える
        poolingObject = Instantiate(_bullet, transform).GetComponent<PoolingObject>();
        poolingObject.InitObject(spawnPosition);
        _poolingObjects.Add(poolingObject);
    }
}
