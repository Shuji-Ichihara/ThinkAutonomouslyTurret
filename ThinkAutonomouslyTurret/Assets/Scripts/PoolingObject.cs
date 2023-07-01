using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    private Transform _transform = null;
    
    /// <summary>
    /// このコンポーネントを持つオブジェクトのプール作成に使用
    /// </summary>
    /// <param name="spawnPosition">このオブジェクトの座標</param>
    public void InitBullet(Vector3 spawnPosition)
    {
        _transform = GetComponent<Transform>();
        _transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}
