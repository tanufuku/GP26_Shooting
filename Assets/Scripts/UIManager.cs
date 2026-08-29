using UnityEngine;
using TMPro;

/// <summary>
/// 画面にスコア・体力・残機を表示する。
/// 【第10回】Canvas / TextMeshPro / static / ToString()
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("スコアを表示するテキスト")]
    [SerializeField] TMP_Text scoreText;

    [Header("体力を表示するテキスト")]
    [SerializeField] TMP_Text hpText;

    [Header("残機を表示するテキスト")]
    [SerializeField] TMP_Text lifeText;

    // 自分自身を覚えておく（static メソッドから使うため）
    static UIManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (scoreText == null)
            Debug.LogError("[UIManager] scoreText が設定されていません。UIManager の Inspector に ScoreText をドラッグしてください");
        if (hpText == null)
            Debug.LogError("[UIManager] hpText が設定されていません。UIManager の Inspector に HPText をドラッグしてください");
        if (lifeText == null)
            Debug.LogError("[UIManager] lifeText が設定されていません。UIManager の Inspector に LifeText をドラッグしてください");

        UpdateAll();
    }

    /// <summary>表示を最新の値に更新する（どこからでも呼べる）</summary>
    public static void UpdateAll()
    {
        if (instance == null) return;
        instance.Refresh();
    }

    void Refresh()
    {
        // 数値を文字列に変換してつなげる（第10回：ToString）
        if (scoreText != null)
            scoreText.text = "SCORE: " + GameManager.score.ToString();

        if (hpText != null)
            hpText.text = "HP: " + PlayerHealth.currentHP.ToString();

        if (lifeText != null)
            lifeText.text = "LIFE: " + PlayerHealth.currentLife.ToString();
    }
}
