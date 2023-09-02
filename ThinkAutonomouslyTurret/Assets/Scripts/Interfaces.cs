using UnityEngine;

/// <summary>
/// オブジェクトプール機能
/// </summary>
public interface PoolFeature
{
    /// <summary>
    /// オブジェクトプール生成
    /// </summary>
    public void InitObjectPool();

    /// <summary>
    /// プール内のオブジェクトを検索、アクティブ化
    /// </summary>
    /// <param name="spawnPosition">スポーンする座標</param>
    /// <param name="number">生成した乱数</param>
    public void ActivateObject(Vector3 spawnPosition, int number = 0);
}