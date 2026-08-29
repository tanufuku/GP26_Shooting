using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーが撃つ弾。
/// 敵やボスに当たるとダメージを与えて消える。壁に当たっても消える。
/// 【第5回】Destroy　【第6回】OnTriggerEnter / タグ判定 / switch文 / GetComponent
/// </summary>
public class Bullet : MonoBehaviour
{
    // この弾のダメージ。撃つときに PlayerShoot が入れてくれる。
    public int damage = 1;

    // 今飛んでいる弾をすべて覚えておく（やり直しのときにまとめて消すため）
    static List<Bullet> allBullets = new List<Bullet>();

    void Awake()
    {
        allBullets.Add(this);
    }

    void OnDestroy()
    {
        allBullets.Remove(this);
    }

    /// <summary>飛んでいる弾をすべて消す（プレイヤー復活時に呼ばれる）</summary>
    public static void DestroyAllBullets()
    {
        // 消しながらリストが変わるので、後ろから消していく
        for (int i = allBullets.Count - 1; i >= 0; i--)
        {
            if (allBullets[i] != null)
            {
                Destroy(allBullets[i].gameObject);
            }
        }
        allBullets.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        // 当たった相手のタグで処理を分ける（第6回：switch文）
        switch (other.tag)
        {
            case "Enemy":
                // 普通の敵ならダメージを与える
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }

                // ボスならボスにダメージを与える
                BossHealth boss = other.GetComponent<BossHealth>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                }

                // 当たった弾は消える
                Destroy(gameObject);
                break;

            case "Wall":
                // 壁に当たったら消えるだけ
                Destroy(gameObject);
                break;
        }
    }
}
