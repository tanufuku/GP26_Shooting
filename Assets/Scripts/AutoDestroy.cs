using UnityEngine;

/// <summary>
/// 出てから一定時間たったら自動で消える。
/// 爆発エフェクトなど、出しっぱなしにすると増え続けてしまうものに付ける。
/// 【第5回】Destroy(オブジェクト, 秒数)
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    [Header("消えるまでの秒数")]
    [SerializeField] float lifeTime = 2f;

    void Start()
    {
        // 指定した秒数がたったら自分を消す
        Destroy(gameObject, lifeTime);
    }
}
