using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の司令塔。
/// ゲームの状態（enum）・スコア・シーン遷移・リスタート処理をまとめて担当する。
/// 【第13回】enum / SceneManager / static
/// </summary>
public class GameManager : MonoBehaviour
{
    // ===== ゲームの状態を表す enum（第13回）=====
    public enum GameState
    {
        Title,      // タイトル画面
        Playing,    // プレイ中
        GameOver,   // ゲームオーバー
        Clear       // クリア
    }

    // 今のゲーム状態（どこからでも見られるように static）
    public static GameState currentState = GameState.Title;

    // 現在のスコア（ゲームオーバーまでリセットしない）
    public static int score = 0;

    // ボスを倒したかどうか（倒したあとは敵を復活させない）
    public static bool bossDefeated = false;

    // 自分自身を覚えておく（static メソッドから呼び出すため）
    static GameManager instance;

    // ----- Inspector で設定する -----

    [Header("ゲーム本編のシーンならチェックを入れる（タイトルやリザルトでは外す）")]
    [SerializeField] bool isGameplayScene = false;

    [Header("プレイヤーの開始位置")]
    [SerializeField] Transform playerStartPoint;

    [Header("プレイヤー")]
    [SerializeField] PlayerHealth player;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // タイトルやリザルト画面では、下の初期化をしない。
        // （ここでスコアを0に戻すと、リザルト画面で結果が表示できなくなるため）
        if (!isGameplayScene) return;

        // 未設定チェック（設定忘れをコンソールで教えてくれる）
        if (playerStartPoint == null)
            Debug.LogError("[GameManager] playerStartPoint が設定されていません。GameManager の Inspector を確認してください");
        if (player == null)
            Debug.LogError("[GameManager] player が設定されていません。GameManager の Inspector に Player をドラッグしてください");

        // ゲーム開始
        currentState = GameState.Playing;
        score = 0;
        bossDefeated = false;
        PlayerStatus.ResetStatus();
    }

    // ================= スコア =================

    /// <summary>スコアを加算する（敵を倒したときに呼ばれる）</summary>
    public static void AddScore(int value)
    {
        score += value;
        UIManager.UpdateAll();
    }

    // ================= プレイヤーが力尽きたとき =================

    /// <summary>
    /// プレイヤーの体力が0になったときに呼ばれる。
    /// 残機が残っていればステージをリセットして復活、無ければゲームオーバー。
    /// </summary>
    public static void OnPlayerDied(bool hasLifeLeft)
    {
        if (currentState != GameState.Playing) return;

        if (hasLifeLeft)
        {
            RestartStage();
        }
        else
        {
            GameOver();
        }
    }

    /// <summary>ステージを最初の状態に戻してプレイヤーを復活させる</summary>
    static void RestartStage()
    {
        if (instance == null) return;

        // 敵をすべて消して、スポーンをリセット（敵がいない状態から再開）
        // ただしボスを倒したあとは、敵を復活させない
        if (bossDefeated)
        {
            EnemySpawner.DestroyAllEnemies();
        }
        else
        {
            EnemySpawner.ResetAllSpawners();
        }

        // 飛んでいる弾をすべて消す（プレイヤーの弾・ボスの弾の両方）
        Bullet.DestroyAllBullets();
        EnemyBullet.DestroyAllBullets();

        // アイテムを元通りに復活させる
        Item.RespawnAllItems();

        // アイテムの効果をリセット
        PlayerStatus.ResetStatus();

        // プレイヤーを開始位置に戻して復活
        if (instance.player != null && instance.playerStartPoint != null)
        {
            instance.player.Revive(instance.playerStartPoint.position, instance.playerStartPoint.rotation);
        }

        UIManager.UpdateAll();
    }

    // ================= ボスを倒したとき =================

    /// <summary>ボス撃破時に呼ばれる。敵の出現を止めて、ゴールを有効化する</summary>
    public static void OnBossDefeated()
    {
        bossDefeated = true;

        // これ以上敵が出てこないようにする
        EnemySpawner.StopAllSpawners();

        // 今いる敵をすべて消す
        EnemySpawner.DestroyAllEnemies();

        // 飛んでいるボスの弾も消す（倒したあとに当たらないように）
        EnemyBullet.DestroyAllBullets();

        // ゴールを有効化する
        GoalPoint.ActivateGoal();

        Debug.Log("ボスを倒した！ 奥のゴールへ向かおう！");
    }

    // ================= シーン遷移 =================

    /// <summary>タイトルの「スタート」ボタンから呼ぶ</summary>
    public void StartGame()
    {
        currentState = GameState.Playing;
        score = 0;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>「タイトルへ戻る」ボタンから呼ぶ</summary>
    public void GoToTitle()
    {
        currentState = GameState.Title;
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>ゲームオーバー</summary>
    public static void GameOver()
    {
        currentState = GameState.GameOver;
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>ゴールに触れたとき（クリア）</summary>
    public static void GameClear()
    {
        currentState = GameState.Clear;
        SceneManager.LoadScene("EndingScene");
    }
}
