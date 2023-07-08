using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Type t = typeof(T);
                _instance = (T)FindObjectOfType(t);
                if (_instance == null)
                {
                    Debug.LogError(t + "をアタッチしている GameObject はありません。");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 他の GameObject にアタッチされているか調べる
        // アタッチされている場合は破棄する
        if (this != Instance)
        {
            Destroy(this);
            Debug.LogError(
                typeof(T)
                + "は既に他の GameObject にアタッチされている為、コンポーネントを破棄しました。"
                + "アタッチされている GameObject は" + _instance.gameObject.name + "です。");
            return;
        }
    }
}
