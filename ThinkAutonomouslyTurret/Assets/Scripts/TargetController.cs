using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    // 的のスコア
    [SerializeField]
    private int _keepScore = 10;
    // 的の耐久値
    public int EndurancePoint = 0;

    private void OnCollisionEnter(Collision other)
    {
        var bullet = other.gameObject.GetComponent<Bullet>();
        HitBullet(bullet);
    }

    /// <summary>
    /// 弾丸が当たったらスコア上昇
    /// </summary>
    /// <param name="bullet">Bullet スクリプト</param>
    private void HitBullet(Bullet bullet)
    {
        EndurancePoint -= bullet.Damage;
        if(EndurancePoint < 0)
        {
            GameManager.Instance.CountUpScore(_keepScore);
            gameObject.SetActive(false);
        }
    }
}
