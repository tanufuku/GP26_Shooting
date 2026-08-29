using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敵を出現させる装置。
/// スポーンポイント1つにつき敵は1体だけ。倒されて消えると、少し待ってから同じ場所にまた出てくる。
/// 【第8回】for文 / 配列 / Prefabから生成　【第9回】List
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("敵を出す場所（複数指定できる）")]
    [SerializeField] Transform[] spawnPoints;

    [Header("出てくる敵の Prefab")]
    [SerializeField] GameObject enemyPrefab;

    [Header("この場所から出る敵の動きパターン")]
    [SerializeField] EnemyMove.MovePattern pattern = EnemyMove.MovePattern.Spin;

    [Header("倒されてから次が出てくるまでの秒数")]
    [SerializeField] float respawnTime = 0f;

    // 今その場所に出ている敵（配列の番号がスポーンポイントと対応する）
    GameObject[] currentEnemies;

    // 次に出すまでの残り時間
    float[] timers;

    // 敵を出すのを止めているかどうか（ボスを倒すと true になる）
    bool stopped = false;

    // 世界にいるすべての Spawner を覚えておく（第9回：List）
    static List<EnemySpawner> allSpawners = new List<EnemySpawner>();

    void OnEnable()
    {
        if (!allSpawners.Contains(this)) allSpawners.Add(this);
    }

    void OnDisable()
    {
        allSpawners.Remove(this);
    }

    void Start()
    {
        if (enemyPrefab == null)
            Debug.LogError("[EnemySpawner] enemyPrefab が設定されていません。" + gameObject.name + " の Inspector に敵の Prefab をドラッグしてください");
        if (spawnPoints == null || spawnPoints.Length == 0)
            Debug.LogError("[EnemySpawner] spawnPoints が空です。" + gameObject.name + " の Inspector に敵を出す場所を登録してください");

        // 配列を、スポーンポイントと同じ数だけ用意する（第8回：配列と .Length）
        currentEnemies = new GameObject[spawnPoints.Length];
        timers = new float[spawnPoints.Length];

        // 最初は全部の場所からすぐ出てくるようにする
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            timers[i] = 0f;
        }
    }

    void Update()
    {
        if (stopped) return;
        if (spawnPoints == null || enemyPrefab == null) return;

        // すべてのスポーンポイントを順番に調べる（第8回：for文）
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // その場所の敵がまだ生きているなら、何もしない
            if (currentEnemies[i] != null) continue;

            // 倒されている間、時間を数える
            timers[i] -= Time.deltaTime;

            if (timers[i] <= 0f)
            {
                SpawnAt(i);
                timers[i] = respawnTime;
            }
        }
    }

    /// <summary>番号 i のスポーンポイントに敵を1体出す</summary>
    void SpawnAt(int i)
    {
        if (spawnPoints[i] == null) return;

        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);

        // この Spawner が決めた動きパターンを敵に教える
        EnemyMove move = enemy.GetComponent<EnemyMove>();
        if (move != null)
        {
            move.pattern = pattern;
        }

        currentEnemies[i] = enemy;
    }

    /// <summary>この Spawner の敵をすべて消して、最初の状態に戻す</summary>
    void ResetThisSpawner()
    {
        stopped = false;

        if (currentEnemies == null) return;

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (currentEnemies[i] != null)
            {
                Destroy(currentEnemies[i]);
                currentEnemies[i] = null;
            }
            timers[i] = respawnTime;
        }
    }

    /// <summary>この Spawner の敵をすべて消す（出現も止める）</summary>
    void StopThisSpawner()
    {
        stopped = true;
    }

    /// <summary>この Spawner が出した敵をすべて消す</summary>
    void DestroyThisSpawnerEnemies()
    {
        if (currentEnemies == null) return;

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (currentEnemies[i] != null)
            {
                Destroy(currentEnemies[i]);
                currentEnemies[i] = null;
            }
        }
    }

    // ===== ここから、どこからでも呼べる static メソッド（第10回）=====

    /// <summary>すべての Spawner をリセットする（プレイヤー復活時）</summary>
    public static void ResetAllSpawners()
    {
        for (int i = 0; i < allSpawners.Count; i++)
        {
            allSpawners[i].ResetThisSpawner();
        }
    }

    /// <summary>すべての Spawner を止める（ボス撃破時）</summary>
    public static void StopAllSpawners()
    {
        for (int i = 0; i < allSpawners.Count; i++)
        {
            allSpawners[i].StopThisSpawner();
        }
    }

    /// <summary>今いる敵をすべて消す（ボス撃破時）</summary>
    public static void DestroyAllEnemies()
    {
        for (int i = 0; i < allSpawners.Count; i++)
        {
            allSpawners[i].DestroyThisSpawnerEnemies();
        }
    }
}
