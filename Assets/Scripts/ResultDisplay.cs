using UnityEngine;
using TMPro;

/// <summary>
/// ゲームオーバー画面・エンディング画面でスコアを表示する。
/// スコアは static なので、シーンが変わっても値が残っている（第10回）。
/// 【第10回】static / ToString()　【第13回】シーンをまたぐ
/// </summary>
public class ResultDisplay : MonoBehaviour
{
    [Header("スコアを表示するテキスト")]
    [SerializeField] TMP_Text scoreText;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("[ResultDisplay] scoreText が設定されていません。Inspector にスコア表示用のテキストをドラッグしてください");
            return;
        }

        scoreText.text = "SCORE: " + GameManager.score.ToString();
    }
}
