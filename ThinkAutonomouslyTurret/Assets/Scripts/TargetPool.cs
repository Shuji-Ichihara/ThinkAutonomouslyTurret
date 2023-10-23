using System.Collections.Generic;
using UnityEngine;

public class TargetPool : SingletonMonoBehaviour<TargetPool>, PoolFeature
{
    // 的の種類
    public GameObject[] TargetType => _targetType;
    [SerializeField]
    private GameObject[] _targetType = { };
    // 的のオブジェクトプールの親
    [SerializeField]
    private Transform[] _poolParentTransforms = { };
    // List<PoolObject> が的の種類分必要 
    private List<PoolingObject>[] _poolingTargetType = { };
    // 備蓄しておく Prefab の個数
    private const int _max = 20;

    /// <summary>
    /// 的のオブジェクトプール生成
    /// </summary>
    public void InitObjectPool()
    {
        PoolingObject poolingObject;
        _poolingTargetType = new List<PoolingObject>[_targetType.Length];
        for (int i = 0; i < _targetType.Length; i++)
        {
            _poolingTargetType[i] = new List<PoolingObject>();
            for (int j = 0; j < _max; j++)
            {
                poolingObject = Instantiate(_targetType[i], _poolParentTransforms[i]).GetComponentInChildren<PoolingObject>();
                poolingObject.gameObject.SetActive(false);
                _poolingTargetType[i].Add(poolingObject);
            }
        }
    }

    /// <summary>
    /// 的オブジェクトの検索、アクティブ化
    /// </summary>
    /// <param name="spawnPosition">オブジェクトを生成する座標</param>
    /// <param name="number">生成された乱数</param>
    public void ActivateObject(Vector3 spawnPosition, int number = 0)
    {
        bool isSpawn = false;
        List<PoolingObject> poolingObjects = _poolingTargetType[number];
        PoolingObject poolingObject;
        GameObject obj;
        for (int i = 0; i < poolingObjects.Count; i++)
        {
            poolingObject = poolingObjects[i];
            if (false == poolingObject.gameObject.activeSelf)
            {
                obj = poolingObject.InitObject(spawnPosition);
                obj.transform.LookAt(CannonController.Instance.gameObject.transform, Vector3.up);
                isSpawn = true;
                break;
            }
        }
        // プール内のオブジェクトが不足した場合、新しく生成しプールに加える
        if (false == isSpawn)
        {
            poolingObject = Instantiate(_targetType[number], _poolParentTransforms[number]).GetComponentInChildren<PoolingObject>();
            obj = poolingObject.InitObject(spawnPosition);
            _poolingTargetType[number].Add(poolingObject);
            obj.transform.LookAt(CannonController.Instance.gameObject.transform, Vector3.up);
        }
    }
}
