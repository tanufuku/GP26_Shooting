using UnityEngine;

/// <summary>
/// カメラがプレイヤーを追いかける。
/// カメラの向きは常に同じ（固定アングル）で、位置だけがプレイヤーを追う。
/// キャラクターがどちらを向いても画面の見え方が変わらないので、操作しやすい。
/// 【第12回】LateUpdate / Vector3.Lerp
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("追いかける相手（Player）")]
    [SerializeField] Transform target;

    [Header("どれくらい後ろから見るか")]
    [SerializeField] float distance = 0f;

    [Header("どれくらい高い位置から見るか")]
    [SerializeField] float height = 0f;

    [Header("ついていく速さ（小さいほど遅れてついてくる）")]
    [SerializeField] float followSpeed = 0f;

    [Header("見る場所の高さの調整")]
    [SerializeField] float lookAtHeight = 1f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("[CameraFollow] target が設定されていません。Main Camera の Inspector に Player をドラッグしてください");
            return;
        }

        // カメラの向きを決める（ここで決めたら、あとはずっと変えない）
        ApplyFixedAngle();

        // 最初からプレイヤーの後ろにいる状態にしておく
        transform.position = target.position + GetOffset();
    }

    // カメラの移動は LateUpdate で行う（プレイヤーが動いた「あと」に動かすため）
    void LateUpdate()
    {
        if (target == null) return;

        // 本当はここにいてほしい、という位置
        Vector3 desiredPosition = target.position + GetOffset();

        // すぐには行かず、少しずつ近づける（これで「遅れてついてくる」動きになる）
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    /// <summary>プレイヤーからどれだけ離れた場所にいるか</summary>
    Vector3 GetOffset()
    {
        // 後ろ（z のマイナス方向）に distance、上に height
        return new Vector3(0f, height, -distance);
    }

    /// <summary>カメラの見下ろす角度を、プレイヤーがちょうど中央に映るように決める</summary>
    void ApplyFixedAngle()
    {
        float pitch = Mathf.Atan2(height - lookAtHeight, distance) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
