using UnityEngine;

/// <summary>
/// BGM と効果音を鳴らす仕組み。
///
/// ★ 音のデータ（AudioClip）は、はじめは空っぽです。
/// 　 自分で好きな音を探してきて、Inspector にドラッグしてください。
/// 　 無料で使える音の例： https://kenney.nl/assets （CC0） / 効果音ラボ など
///
/// 【第11回】AudioSource / AudioClip / Play / PlayOneShot　【第10回】static
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("BGM を鳴らす AudioSource")]
    [SerializeField] AudioSource bgmSource;

    [Header("効果音を鳴らす AudioSource")]
    [SerializeField] AudioSource seSource;

    [Header("BGM の音データ（自分で入れる）")]
    [SerializeField] AudioClip bgmClip;

    [Header("弾を撃つ音（自分で入れる）")]
    [SerializeField] AudioClip shootClip;

    [Header("爆発の音（自分で入れる）")]
    [SerializeField] AudioClip explosionClip;

    [Header("ダメージを受けた音（自分で入れる）")]
    [SerializeField] AudioClip damageClip;

    [Header("アイテムを取った音（自分で入れる）")]
    [SerializeField] AudioClip itemClip;

    static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (bgmSource == null)
            Debug.LogError("[AudioManager] bgmSource が設定されていません。AudioManager の Inspector に AudioSource をドラッグしてください");
        if (seSource == null)
            Debug.LogError("[AudioManager] seSource が設定されていません。AudioManager の Inspector に AudioSource をドラッグしてください");

        PlayBGM();
    }

    /// <summary>BGM を鳴らす（ループ再生）</summary>
    void PlayBGM()
    {
        if (bgmSource == null || bgmClip == null) return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>効果音を1回鳴らす（重なって鳴らせる）</summary>
    static void PlaySE(AudioClip clip)
    {
        if (instance == null) return;
        if (instance.seSource == null || clip == null) return;

        instance.seSource.PlayOneShot(clip);
    }

    // ===== どこからでも呼べる効果音 =====

    public static void PlayShootSE() { if (instance != null) PlaySE(instance.shootClip); }
    public static void PlayExplosionSE() { if (instance != null) PlaySE(instance.explosionClip); }
    public static void PlayDamageSE() { if (instance != null) PlaySE(instance.damageClip); }
    public static void PlayItemSE() { if (instance != null) PlaySE(instance.itemClip); }
}
