using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    #region Move
    // Bullet に加える力
    [SerializeField]
    private float _bulletMoveForce = 60.0f;
    // Bullet の移動距離
    private float _moveBulletDistance = 0.0f;
    #endregion
    // 弾のダメージ
    [SerializeField]
    private int _damage = 10;
    public int Damage => _damage;

    private Rigidbody _rb = null;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FixedUpdate()
    {
        MoveBullet();
    }

    /// <summary>
    /// Bullet プレハブの移動
    /// </summary>
    private void MoveBullet()
    {
        _rb.AddForce(CannonController.Instance.BulletSpawnPoint.up * _bulletMoveForce, ForceMode.Impulse);
        _moveBulletDistance = Vector3.Distance(transform.position, GameSceneManager.Instance.Cannon.transform.position);
        // Cannon との距離が離れたら非アクティブ化する
        if (_moveBulletDistance > 200.0f)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 与えた力を 0 にする
    /// </summary>
    private void OnDisable()
    {
        _rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            gameObject.SetActive(false);
        }
    }
}
