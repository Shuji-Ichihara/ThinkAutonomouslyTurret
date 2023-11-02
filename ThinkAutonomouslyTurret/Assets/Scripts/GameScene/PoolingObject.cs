using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    private Transform _transform = null;
    
    /// <summary>
    /// このコンポーネントがアタッチされているオブジェクトのプール作成に使用
    /// </summary>
    /// <param name="spawnPosition">このオブジェクトの座標</param>
    public GameObject InitObject(Vector3 spawnPosition)
    {
        _transform = GetComponent<Transform>();
        _transform.position = spawnPosition;
        gameObject.SetActive(true);
        return gameObject;
    }
}
