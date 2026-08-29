using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 拾うと強くなるアイテム。
/// 種類は Inspector のドロップダウンで選ぶ。取ると消えるが、プレイヤーが復活すると元通り現れる。
/// 見た目のくるくる回転とふわふわ拡大縮小は Animation が担当する（このスクリプトは触らない）。
/// 【第6回】OnTriggerEnter / タグ判定　【第13回】enum
/// </summary>
public class Item : MonoBehaviour
{
    // ===== アイテムの種類（第13回：enum）=====
    public enum ItemType
    {
        Heal,       // 緑：体力回復
        RapidFire,  // 黄：連射（発射間隔が短くなる）
        PowerUp,    // 紫：ダメージアップ
        SpeedUp     // 水色：移動速度アップ
    }

    [Header("アイテムの種類")]
    [SerializeField] ItemType type = ItemType.Heal;

    [Header("【Heal】回復する量")]
    [SerializeField] int healAmount = 0;

    [Header("【RapidFire】発射間隔を短くする量（秒）")]
    [SerializeField] float intervalDownAmount = 0f;

    [Header("【PowerUp】ダメージを増やす量")]
    [SerializeField] int damageUpAmount = 0;

    [Header("【SpeedUp】移動速度を増やす量")]
    [SerializeField] float speedUpAmount = 0f;

    // 世界にあるすべてのアイテムを覚えておく（あとで復活させるため）
    static List<Item> allItems = new List<Item>();

    void Awake()
    {
        if (!allItems.Contains(this)) allItems.Add(this);
    }

    void OnDestroy()
    {
        allItems.Remove(this);
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが触れたときだけ反応する
        if (other.tag != "Player") return;

        // 種類ごとに効果を変える（第13回：enum ＋ switch文）
        switch (type)
        {
            case ItemType.Heal:
                PlayerHealth hp = other.GetComponent<PlayerHealth>();
                if (hp != null) hp.Heal(healAmount);
                break;

            case ItemType.RapidFire:
                PlayerStatus.intervalDown += intervalDownAmount;
                break;

            case ItemType.PowerUp:
                PlayerStatus.damageBonus += damageUpAmount;
                break;

            case ItemType.SpeedUp:
                PlayerStatus.speedBonus += speedUpAmount;
                break;
        }

        AudioManager.PlayItemSE();

        // 取ったアイテムは見えなくする（消さずに隠しておく）
        gameObject.SetActive(false);
    }

    /// <summary>すべてのアイテムを元通り出す（プレイヤー復活時に呼ばれる）</summary>
    public static void RespawnAllItems()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i] != null)
            {
                allItems[i].gameObject.SetActive(true);
            }
        }
    }
}
