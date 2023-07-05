using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet = null;

    private List<PoolingObject> _poolingObjects = new List<PoolingObject>();
    // 備蓄しておく Prefab の個数
    private const int _max = 100;

    /// <summary>
    /// オブジェクトプール生成
    /// </summary>
    public void InitPoolBullet()
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
    /// プール内のオブジェクトを検索、
    /// </summary>
    /// <param name="spawnPosition">スポーンする座標</param>
    public void ActivateObject(Vector3 spawnPosition)
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
        _bullet.transform.parent = transform;
        poolingObject.InitObject(spawnPosition);
        _poolingObjects.Add(poolingObject);
    }
}
