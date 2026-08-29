using UnityEngine;

/// <summary>
/// 敵の体力を管理する。
/// 弾が当たると体力が減り、0 になると爆発して消え、スコアが入る。
/// 【第6回】ダメージ処理　【第10回】スコア加算　【第12回】爆発エフェクト
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [Header("敵の体力")]
    [SerializeField] int maxHP = 0;

    [Header("倒したときに入るスコア")]
    [SerializeField] int scoreValue = 0;

    [Header("倒したときに出す爆発の Prefab")]
    [SerializeField] GameObject explosionPrefab;

    int currentHP;

    void Start()
    {
        currentHP = maxHP;

        if (explosionPrefab == null)
            Debug.LogError("[EnemyHealth] explosionPrefab が設定されていません。敵の Prefab の Inspector に爆発 Prefab をドラッグしてください（" + gameObject.name + "）");
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

    /// <summary>倒された</summary>
    void Die()
    {
        // 爆発を出す（第12回）
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // スコアを加算する（第10回）
        GameManager.AddScore(scoreValue);

        AudioManager.PlayExplosionSE();

        // 自分を消す
        Destroy(gameObject);
    }
}
