using UnityEngine;

/// <summary>
/// プレイヤーの移動。
/// W / A / S / D を押すと、その方向を向いて進む。
/// 弾はいつもキャラクターの正面（鼻の向き）に飛ぶので、向いた方向に撃てる。
/// 【第4回】if文 / Input.GetKey / Vector3 / transform.Translate / Time.deltaTime
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [Header("移動の速さ")]
    [SerializeField] float moveSpeed = 0f;

    void Update()
    {
        // アイテムで上がった分を足した「今の速さ」
        float speed = moveSpeed + PlayerStatus.speedBonus;

        // ----- 押されているキーから、進みたい方向を組み立てる -----
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;   // 奥へ
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;      // 手前へ
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;      // 左へ
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;     // 右へ
        }

        // どのキーも押されていなければ、何もしない
        if (direction == Vector3.zero) return;

        // 斜め（W＋D など）のときに速くなりすぎないよう、長さを1にそろえる
        direction = direction.normalized;

        // その方向を向く
        transform.forward = direction;

        // 向いた方向へ進む
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
