using UnityEngine;

/// <summary>
/// スペースキーで弾を撃つ。
/// 弾はキャラクターの正面（鼻の位置＝ShootPoint）から飛んでいく。
/// 【第4回】Time.deltaTime　【第5回】Prefab / Instantiate / メソッド / Destroy
/// </summary>
public class PlayerShoot : MonoBehaviour
{
    [Header("弾の Prefab")]
    [SerializeField] GameObject bulletPrefab;

    [Header("弾が出る位置（鼻の ShootPoint）")]
    [SerializeField] Transform shootPoint;

    [Header("弾の速さ")]
    [SerializeField] float bulletSpeed = 0f;

    [Header("弾が消えるまでの秒数")]
    [SerializeField] float bulletLifeTime = 0f;

    [Header("次の弾を撃てるまでの間隔（秒）")]
    [SerializeField] float shootInterval = 0f;

    [Header("弾1発のダメージ")]
    [SerializeField] int bulletDamage = 0;

    // 前に撃ってからの経過時間
    float timer = 0f;

    void Start()
    {
        if (bulletPrefab == null)
            Debug.LogError("[PlayerShoot] bulletPrefab が設定されていません。Player の Inspector に弾の Prefab をドラッグしてください");
        if (shootPoint == null)
            Debug.LogError("[PlayerShoot] shootPoint が設定されていません。Player の Inspector に ShootPoint をドラッグしてください");
    }

    void Update()
    {
        // 時間を数える
        timer += Time.deltaTime;

        // アイテムで短くなった分を引いた「今の発射間隔」
        float interval = shootInterval - PlayerStatus.intervalDown;

        // スペースキーを押している間、間隔を空けて撃ち続ける
        if (Input.GetKey(KeyCode.Space) && timer >= interval)
        {
            Shoot();
            timer = 0f;
        }
    }

    /// <summary>弾を1発撃つ</summary>
    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // ① Prefab から弾を作る
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // ② 弾にダメージを教える（アイテムで上がった分を足す）
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.damage = bulletDamage + PlayerStatus.damageBonus;
        }

        // ③ 前に飛ばす
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * bulletSpeed;
        }

        // ④ 一定時間で消す（撃ちっぱなしで増え続けないように）
        Destroy(bullet, bulletLifeTime);

        // 発射音を鳴らす
        AudioManager.PlayShootSE();
    }
}
