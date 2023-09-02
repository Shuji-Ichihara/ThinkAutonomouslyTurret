using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : SingletonMonoBehaviour<CannonController>
{
    #region Refarence
    // 弾を発射する座標
    [SerializeField]
    private Transform _bulletSpawnPoint = null;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;
    // 砲台のピボット
    [SerializeField]
    private Transform _CannonPivot = null;
    // 砲身のピボット
    [SerializeField]
    private Transform _burralRoot = null;
    #endregion
    #region MoveSpeed
    // 砲台が回転するスピード
    [SerializeField]
    private float _canonRotateSpeed = 60.0f;
    // 砲身が回転するスピード
    [SerializeField]
    private float _burarlRotateSpeed = 5.0f;
    #endregion
    // 弾のオブジェクトプール
    private BulletPool _bulletPool = null;

    // Start is called before the first frame update
    void Start()
    {
        _bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
    }

    // Update is called once per frame
    void Update()
    {
        ShotBullet();
        RotateCanon();
        RotateBurralAngle();
    }

    /// <summary>
    /// 砲台の回転
    /// </summary>
    private void RotateCanon()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _CannonPivot.rotation *= Quaternion.AngleAxis(_canonRotateSpeed * Time.deltaTime, Vector3.down);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _CannonPivot.rotation *= Quaternion.AngleAxis(_canonRotateSpeed * Time.deltaTime, Vector3.up);
        }
    }

    /// <summary>
    /// 砲身の角度を調節
    /// </summary>
    private void RotateBurralAngle()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // unity の回転軸は左手座標系のため、時計回りになる
            float x = (_burralRoot.localRotation *= Quaternion.AngleAxis(_burarlRotateSpeed * Time.deltaTime, Vector3.left)).x;
            _burralRoot.localEulerAngles -= new Vector3(x, 0.0f, 0.0f);
            if(_burralRoot.localEulerAngles.x < 360.0f - 15.0f)
            {
                _burralRoot.localEulerAngles = new Vector3(360.0f - 15.0f, 0.0f, 0.0f);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _burralRoot.rotation *= Quaternion.AngleAxis(_burarlRotateSpeed * Time.deltaTime, Vector3.right);
            if (_burralRoot.localEulerAngles.x > 0.0f && _burralRoot.localEulerAngles.x < 360.0f - 15.0f)
            {
                _burralRoot.localEulerAngles = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    private void ShotBullet()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _bulletPool.ActivateObject(_bulletSpawnPoint.position);
        }
    }
}
