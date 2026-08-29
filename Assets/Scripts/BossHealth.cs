using UnityEngine;

/// <summary>
/// ボスの体力を管理する。
/// 倒すと敵の出現がすべて止まり、今いる敵も消え、奥のゴールが有効になる。
/// 【第6回】ダメージ処理　【第13回】ゲームの進行
/// </summary>
public class BossHealth : MonoBehaviour
{
    [Header("ボスの体力")]
    [SerializeField] int maxHP = 0;

    [Header("倒したときに入るスコア")]
    [SerializeField] int scoreValue = 0;

    [Header("倒したときに出す爆発の Prefab")]
    [SerializeField] GameObject explosionPrefab;

    [Header("爆発を何倍の大きさで出すか")]
    [SerializeField] float explosionScale = 3f;

    int currentHP;

    void Start()
    {
        currentHP = maxHP;

        if (explosionPrefab == null)
            Debug.LogError("[BossHealth] explosionPrefab が設定されていません。Boss の Inspector に爆発 Prefab をドラッグしてください");
    }

    /// <summary>ダメージを受ける（Bullet から呼ばれる）</summary>
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>ボスが倒された</summary>
    void Die()
    {
        // 大きめの爆発を出す
        if (explosionPrefab != null)
        {
            GameObject effect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            effect.transform.localScale = Vector3.one * explosionScale;
        }

        GameManager.AddScore(scoreValue);
        AudioManager.PlayExplosionSE();

        // 敵の出現停止・既存の敵の消滅・ゴールの有効化をまとめて依頼する
        GameManager.OnBossDefeated();

        Destroy(gameObject);
    }
}
