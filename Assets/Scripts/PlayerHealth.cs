using UnityEngine;

/// <summary>
/// プレイヤーの体力・残機・無敵時間を管理する。
/// 敵やボスの弾に当たると体力が1減り、しばらく点滅して無敵になる。
/// 体力が0になると残機が1減ってステージの最初からやり直し。残機が無くなるとゲームオーバー。
/// 【第6回】OnTriggerEnter / タグ判定　【第10回】UIとの連携　【第13回】ゲーム進行
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("体力の最大値")]
    [SerializeField] int maxHP = 0;

    [Header("残機の数")]
    [SerializeField] int maxLife = 0;

    [Header("ダメージを受けたあとの無敵時間（秒）")]
    [SerializeField] float invincibleTime = 0f;

    [Header("無敵中に点滅する間隔（秒）")]
    [SerializeField] float blinkInterval = 0f;

    // 現在の体力と残機（他のスクリプトから読めるように public）
    public static int currentHP;
    public static int currentLife;

    // 無敵中かどうか
    bool isInvincible = false;
    float invincibleTimer = 0f;
    float blinkTimer = 0f;

    // 見た目のパーツ（点滅させるために全部覚えておく）
    Renderer[] renderers;

    void Start()
    {
        // 自分と子供にある見た目パーツをすべて集める（雪だるまなので複数ある）
        renderers = GetComponentsInChildren<Renderer>();

        currentHP = maxHP;
        currentLife = maxLife;

        UIManager.UpdateAll();
    }

    void Update()
    {
        // ----- 無敵時間の処理（点滅させる）-----
        if (isInvincible)
        {
            if (renderers == null || renderers.Length == 0) return;

            invincibleTimer -= Time.deltaTime;
            blinkTimer -= Time.deltaTime;

            // 一定間隔で見た目を出したり消したりする
            if (blinkTimer <= 0f)
            {
                SetVisible(!renderers[0].enabled);
                blinkTimer = blinkInterval;
            }

            // 無敵時間が終わったら元に戻す
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                SetVisible(true);
            }
        }
    }

    /// <summary>敵やボスの弾に触れたとき</summary>
    void OnTriggerEnter(Collider other)
    {
        // 当たった相手のタグで処理を分ける（第6回：switch文）
        switch (other.tag)
        {
            case "Enemy":
                TakeDamage(1);
                break;

            case "EnemyBullet":
                TakeDamage(1);
                Destroy(other.gameObject);   // 当たった弾は消す
                break;
        }
    }

    /// <summary>ダメージを受ける</summary>
    public void TakeDamage(int amount)
    {
        // 無敵中はダメージを受けない
        if (isInvincible) return;
        if (GameManager.currentState != GameManager.GameState.Playing) return;

        currentHP -= amount;
        UIManager.UpdateAll();
        AudioManager.PlayDamageSE();

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartInvincible();
        }
    }

    /// <summary>体力が0になった</summary>
    void Die()
    {
        currentLife--;
        UIManager.UpdateAll();

        // 残機が残っていれば復活、無ければゲームオーバー
        GameManager.OnPlayerDied(currentLife > 0);
    }

    /// <summary>開始位置から復活する（GameManager から呼ばれる）</summary>
    public void Revive(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        currentHP = maxHP;

        // 復活直後はしばらく無敵にする
        StartInvincible();

        UIManager.UpdateAll();
    }

    /// <summary>無敵時間をはじめる</summary>
    void StartInvincible()
    {
        isInvincible = true;
        invincibleTimer = invincibleTime;
        blinkTimer = blinkInterval;
    }

    /// <summary>見た目のパーツをまとめて表示／非表示する</summary>
    void SetVisible(bool visible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = visible;
        }
    }

    /// <summary>体力を回復する（緑のアイテム）</summary>
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        UIManager.UpdateAll();
    }

    /// <summary>UI 表示用に最大値を教える</summary>
    public int GetMaxHP() { return maxHP; }
}
