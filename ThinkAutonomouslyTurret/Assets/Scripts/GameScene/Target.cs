using UnityEngine;

public class Target : MonoBehaviour
{
    // 的のスコア
    [SerializeField]
    private int _keepScore = 10;
    // 的の耐久値
    [SerializeField]
    private int _endurancePoint = 0;

    private void Start()
    {
        gameObject.tag = "Target";
    }

    private void OnCollisionEnter(Collision other)
    {
        var shell = other.gameObject.GetComponent<Shell>();
        if(shell != null)
            HitShell(shell);
    }

    /// <summary>
    /// 的を破壊したらスコア上昇
    /// </summary>
    /// <param name="shell">Bullet スクリプト</param>
    private void HitShell(Shell shell)
    {
        int damage = shell.Damage;
        _endurancePoint -= damage;
        if (_endurancePoint < 0)
        {
            GameSceneManager.Instance.CountUpScore(_keepScore);
            GameUIManager.Instance.CallPopUpScoreText(_keepScore);
            gameObject.SetActive(false);
            return;
        }
        GameUIManager.Instance.CallPopUpDamageText(damage);
    }
}
