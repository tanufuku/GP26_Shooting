using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ボスが撃つ弾。2種類ある。
/// 　Straight … まっすぐ飛ぶ
/// 　Homing   … 少しずつプレイヤーの方へ向きを変えながら飛ぶ
/// 【第5回】弾を飛ばす　【第9回】プレイヤーの方を向く　【第12回】Lerp でなめらかに変える
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    // ===== 弾の種類（第13回：enum）=====
    public enum BulletType
    {
        Straight,   // まっすぐ
        Homing      // 少し追いかける
    }

    [Header("弾の種類")]
    [SerializeField] BulletType type = BulletType.Straight;

    [Header("飛ぶ速さ")]
    [SerializeField] float speed = 0f;

    [Header("【Homing】どれくらい強く追いかけるか（大きいほど曲がる）")]
    [SerializeField] float homingRate = 0f;

    Transform player;

    // 出ている弾をすべて覚えておく（リスタート時にまとめて消すため）
    static List<EnemyBullet> allBullets = new List<EnemyBullet>();

    void Awake()
    {
        allBullets.Add(this);
    }

    void OnDestroy()
    {
        allBullets.Remove(this);
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        // 追いかけるタイプなら、少しずつプレイヤーの方へ向きを変える
        if (type == BulletType.Homing && player != null)
        {
            // プレイヤーへ向かう方向
            Vector3 toPlayer = (player.position - transform.position).normalized;

            // 今の向きから、その方向へ少しだけ近づける
            // （カメラがなめらかに追いかけたのと同じ Lerp の考え方 ― 第12回）
            transform.forward = Vector3.Lerp(transform.forward, toPlayer, homingRate * Time.deltaTime);
        }

        // 向いている方向へ進む
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーに当たったとき、壁に当たったときは消える
        // （プレイヤーのダメージ処理は PlayerHealth 側で行う）
        switch (other.tag)
        {
            case "Player":
            case "Wall":
                Destroy(gameObject);
                break;
        }
    }

    /// <summary>出ている弾をすべて消す（プレイヤー復活時に呼ばれる）</summary>
    public static void DestroyAllBullets()
    {
        // 消しながらリストが変化するので、後ろから消していく
        for (int i = allBullets.Count - 1; i >= 0; i--)
        {
            if (allBullets[i] != null)
            {
                Destroy(allBullets[i].gameObject);
            }
        }
        allBullets.Clear();
    }
}
