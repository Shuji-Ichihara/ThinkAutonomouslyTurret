using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class CannonController : SingletonMonoBehaviour<CannonController>
{
    #region Refarence
    // 弾を発射する座標
    public Transform ShellSpawnPoint => _shellSpawnPoint;
    [SerializeField]
    private Transform _shellSpawnPoint = null;
    // 砲台のピボット
    [SerializeField]
    private Transform _cannonPivot = null;
    public Transform BurralRoot => _burralRoot;
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
    private ShellPool _shellPool = null;

    // Start is called before the first frame update
    void Start()
    {
        // null チェック
        if(_shellSpawnPoint == null)
        {
            GameObject obj = GameObject.Find("ShellSpawnPoint");
            _shellSpawnPoint = obj.GetComponent<Transform>();
        }
        if(_cannonPivot == null)
        {
            GameObject obj = GameObject.Find("CannonPivot");
            _cannonPivot = obj.GetComponent<Transform>();
        }
        if(_burralRoot == null)
        {
            GameObject obj = GameObject.Find("BurralRoot");
            _burralRoot = obj.GetComponent<Transform>();
        }
        _shellPool = GameObject.Find("ShellPool").GetComponent<ShellPool>();
        CallShotShell();
    }

    // Update is called once per frame
    void Update()
    {
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
            _cannonPivot.rotation *= Quaternion.AngleAxis(_canonRotateSpeed * Time.deltaTime, Vector3.down);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _cannonPivot.rotation *= Quaternion.AngleAxis(_canonRotateSpeed * Time.deltaTime, Vector3.up);
        }
    }

    /// <summary>
    /// 砲身の角度を調整
    /// </summary>
    private void RotateBurralAngle()
    {
        float burralRotationLimit = 20.0f, maxRotation = 360.0f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // unity の回転軸は左手座標系のため、時計回りになる
            float x = (_burralRoot.localRotation *= Quaternion.AngleAxis(_burarlRotateSpeed * Time.deltaTime, Vector3.left)).x;
            _burralRoot.localEulerAngles -= new Vector3(x, 0.0f, 0.0f);
            if (_burralRoot.localEulerAngles.x < maxRotation - burralRotationLimit)
            {
                _burralRoot.localEulerAngles = new Vector3(maxRotation - burralRotationLimit, 0.0f, 0.0f);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _burralRoot.rotation *= Quaternion.AngleAxis(_burarlRotateSpeed * Time.deltaTime, Vector3.right);
            if (_burralRoot.localEulerAngles.x > 0.0f && _burralRoot.localEulerAngles.x < maxRotation - burralRotationLimit)
            {
                _burralRoot.localEulerAngles = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    /// <param name="cts">キャンセル処理用のトークン</param>
    private async UniTask ShotShell(CancellationTokenSource cts = default)
    {
        while (GameSceneManager.Instance.GameTime > 0.0f)
        {
            try
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    AudioManager.Instance.PlaySE(SEType.ShotShell);
                    _shellPool.ActivateObject(_shellSpawnPoint.position);
                }
                await UniTask.WaitForSeconds(Time.fixedDeltaTime * 5, cancellationToken: cts.Token);
            }
            catch (Exception)
            {
                continue;
            }
        }
    }

    /// <summary>
    /// ShotBullet を呼び出す
    /// </summary>
    private void CallShotShell()
    {
        CancellationTokenSource shotShellToken = new CancellationTokenSource();
        ShotShell(shotShellToken).Forget();
    }
}
