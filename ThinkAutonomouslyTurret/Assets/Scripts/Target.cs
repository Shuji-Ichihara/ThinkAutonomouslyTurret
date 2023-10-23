using UnityEngine;

public class Target : MonoBehaviour
{
    // 的のスコア
    [SerializeField]
    private int _keepScore = 10;
    // 的の耐久値
    [SerializeField]
    private int _endurancePoint = 0;

    private void OnCollisionEnter(Collision other)
    {
        var bullet = other.gameObject.GetComponent<Bullet>();
        HitBullet(bullet);
    }

    /// <summary>
    /// 的を破壊したらスコア上昇
    /// </summary>
    /// <param name="bullet">Bullet スクリプト</param>
    private void HitBullet(Bullet bullet)
    {
        _endurancePoint -= bullet.Damage;
        if(_endurancePoint < 0)
        {
            GameManager.Instance.CountUpScore(_keepScore);
            gameObject.SetActive(false);
        }
    }
}
