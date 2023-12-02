using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM の種類の羅列
/// </summary>
public enum BGMType
{
    TitleBGM,
    GameBGM,
    ResultBGM,
}

/// <summary>
/// SE の種類の羅列
/// </summary>
public enum SEType
{
    // TODO: 名称変更予定
    MoveCannon,
    ShotShell,
    BreakTarget,
    CountUpScore,
    PreviewText,
}

/// <summary>
/// BGM の音声アセットの詳細設定
/// </summary>
[System.Serializable]
public struct BGMData
{
    [Header("BGM の種類")]
    public BGMType BGMType;
    [Header("音声アセット")]
    public AudioClip Clip;
    [Header("BGM のボリューム")]
    [Range(0.0f, 1.0f)]
    public float Volume;
    [Header("ループの可否")]
    public bool IsLoop;
}

/// <summary>
/// SE の音声アセットの詳細設定
/// </summary>
[System.Serializable]
public struct SEData
{
    [Header("SE の種類")]
    public SEType SeType;
    [Header("音声アセット")]
    public AudioClip Clip;
    [Header("SE のボリューム")]
    [Range(0.0f, 1.0f)]
    public float Volume;
}

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    // BGM を再生するソース
    [SerializeField]
    private AudioSource _bgmAudioSource = null;
    // SE を再生するソース
    [SerializeField]
    private AudioSource _seAudioSource = null;
    // BGM データのリスト
    [SerializeField]
    private List<BGMData> _bgmDataList = new List<BGMData>();
    // SE データのリスト
    [SerializeField]
    private List<SEData> _seDataList = new List<SEData>();

    new private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// BGM 再生
    /// </summary>
    /// <param name="type">再生する BGM の種類</param>
    public void PlayBGM(BGMType type)
    {
        var bgmData = _bgmDataList[(int)type];
        _bgmAudioSource.clip = bgmData.Clip;
        _bgmAudioSource.volume = bgmData.Volume;
        _bgmAudioSource.loop = bgmData.IsLoop;
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// SE 再生
    /// </summary>
    /// <param name="type">再生する SE の種類</param>
    public void PlaySE(SEType type)
    {
        var seData = _seDataList[(int)type];
        _seAudioSource.clip = seData.Clip;
        _seAudioSource.volume = seData.Volume;
        _seAudioSource.PlayOneShot(seData.Clip);
    }
}
