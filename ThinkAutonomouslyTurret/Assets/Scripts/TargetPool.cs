using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetPool : SingletonMonoBehaviour<TargetPool>, PoolFeature
{
    // 的の種類
    [SerializeField]
    private GameObject[] _targetType = { };
    public GameObject[] TargetType => _targetType;
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
        List<PoolingObject> poolingObjects = new List<PoolingObject>();
        _poolingTargetType = new List<PoolingObject>[_targetType.Length];
        for (int i = 0; i < _targetType.Length; i++)
        {
            _poolingTargetType[i] = poolingObjects;
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
        List<PoolingObject> poolingObjects = GetPoolingTargetType(number);
        PoolingObject poolingObject;
        GameObject obj;
        for (int i = 0; i < poolingObjects.Count; i++)
        {
            poolingObject = poolingObjects[i];
            if (false == poolingObject.gameObject.activeSelf)
            {
                obj = poolingObject.InitObject(spawnPosition);
                obj.transform.LookAt(CannonController.Instance.gameObject.transform, Vector3.forward);
                return;
            }
        }
        // 万一、プール内のオブジェクトが不足した場合、新しく生成しプールに加える
        poolingObject = Instantiate(_targetType[number], _poolParentTransforms[number]).GetComponentInChildren<PoolingObject>();
        obj = poolingObject.InitObject(spawnPosition);
        _poolingTargetType[number].Add(poolingObject);
        obj.transform.LookAt(CannonController.Instance.gameObject.transform, Vector3.forward);
    }

    /// <summary>
    /// 的の種類を確定させ、それのオブジェクトプールを返す
    /// </summary>
    /// <param name="index">配列のインデックス</param>
    /// <returns>的のオブジェクトプール</returns>
    private List<PoolingObject> GetPoolingTargetType(int index)
    {
        return _poolingTargetType[index];
    }
}
