using UnityEngine;

/// <summary>
/// ボスが弾を撃つ。
/// まっすぐ飛ぶ弾と、少しだけ追いかけてくる弾を交互に撃つ。
/// 【第5回】Instantiate / Destroy　【第8回】時間を数えて繰り返す
/// </summary>
public class BossAttack : MonoBehaviour
{
    [Header("まっすぐ飛ぶ弾の Prefab")]
    [SerializeField] GameObject straightBulletPrefab;

    [Header("追いかけてくる弾の Prefab")]
    [SerializeField] GameObject homingBulletPrefab;

    [Header("弾が出る位置")]
    [SerializeField] Transform firePoint;

    [Header("弾を撃つ間隔（秒）")]
    [SerializeField] float attackInterval = 0f;

    [Header("弾が消えるまでの秒数")]
    [SerializeField] float bulletLifeTime = 0f;

    // 前に撃ってからの経過時間
    float timer = 0f;

    // 次はどちらの弾を撃つか（true でまっすぐ、false で追いかける）
    bool shootStraight = true;

    void Start()
    {
        if (straightBulletPrefab == null)
            Debug.LogError("[BossAttack] straightBulletPrefab が設定されていません。Boss の Inspector を確認してください");
        if (homingBulletPrefab == null)
            Debug.LogError("[BossAttack] homingBulletPrefab が設定されていません。Boss の Inspector を確認してください");
        if (firePoint == null)
            Debug.LogError("[BossAttack] firePoint が設定されていません。Boss の Inspector に FirePoint をドラッグしてください");
    }

    void Update()
    {
        // プレイ中以外は撃たない
        if (GameManager.currentState != GameManager.GameState.Playing) return;

        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    /// <summary>弾を1発撃つ（2種類を交互に）</summary>
    void Shoot()
    {
        if (firePoint == null) return;

        // 交互に使う Prefab を決める
        GameObject prefab = shootStraight ? straightBulletPrefab : homingBulletPrefab;
        shootStraight = !shootStraight;

        if (prefab == null) return;

        GameObject bullet = Instantiate(prefab, firePoint.position, firePoint.rotation);

        // 一定時間で消す
        Destroy(bullet, bulletLifeTime);
    }
}
