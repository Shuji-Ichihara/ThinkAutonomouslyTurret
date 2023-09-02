using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    #region Move
    // Bullet の速度
    [SerializeField]
    private float _bulletSpeed = 60.0f;
    // Bullet の移動距離
    private float _moveBulletDistance = 0.0f;
    #endregion

    [SerializeField]
    private int _damage = 10;
    public int Damage => _damage;

    private Rigidbody _rb = null;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
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
        _moveBulletDistance += _bulletSpeed * Time.deltaTime;
        transform.position += CannonController.Instance.BulletSpawnPoint.up * _moveBulletDistance;
        if(_moveBulletDistance > 200.0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _moveBulletDistance = 0.0f;
    }
}
