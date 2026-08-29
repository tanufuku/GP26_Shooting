using UnityEngine;

/// <summary>
/// アイテムで強化された分を覚えておく場所。
/// 移動・射撃・弾のダメージが、それぞれここの値を見に来る。
/// プレイヤーが力尽きて復活したときは、ここを 0 に戻すだけで強化がリセットされる。
/// 【第3回】変数　【第10回】static
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    // ===== アイテムで上がった分（static でどこからでも見られる）=====

    /// <summary>移動速度の上昇分（水色のアイテム）</summary>
    public static float speedBonus = 0f;

    /// <summary>弾のダメージの上昇分（紫のアイテム）</summary>
    public static int damageBonus = 0;

    /// <summary>発射間隔の短縮分（黄色のアイテム）</summary>
    public static float intervalDown = 0f;

    /// <summary>強化をすべてリセットする（復活したときに呼ばれる）</summary>
    public static void ResetStatus()
    {
        speedBonus = 0f;
        damageBonus = 0;
        intervalDown = 0f;
    }
}
