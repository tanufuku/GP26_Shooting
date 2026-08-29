using UnityEngine;

/// <summary>
/// 敵の動き。Inspector のドロップダウンで4つのパターンから選べる。
/// 　Spin      … その場でくるくる回る
/// 　LeftRight … その場で左右に行ったり来たり
/// 　Circle    … その場で円を描いて回る
/// 　Chase     … じっとしていて、プレイヤーが近づいたら追いかけてくる
/// 【第9回】追尾AI / LookAt / Vector3.Distance　【第13回】enum
/// </summary>
public class EnemyMove : MonoBehaviour
{
    // ===== 動きのパターン（第13回：enum）=====
    public enum MovePattern
    {
        Spin,       // その場でくるくる
        LeftRight,  // 左右にループ
        Circle,     // 円を描く
        Chase       // 近づいたら追いかける
    }

    [Header("動きのパターン")]
    public MovePattern pattern = MovePattern.Spin;

    [Header("【Spin】回る速さ（1秒あたりの角度）")]
    [SerializeField] float spinSpeed = 0f;

    [Header("【LeftRight】左右に動く幅")]
    [SerializeField] float moveRange = 0f;

    [Header("【LeftRight】左右に動く速さ")]
    [SerializeField] float moveSpeed = 0f;

    [Header("【Circle】円の半径")]
    [SerializeField] float circleRadius = 0f;

    [Header("【Circle】円を回る速さ（1秒あたりの角度）")]
    [SerializeField] float circleSpeed = 0f;

    [Header("【Circle】円の中心をずらす量（出現位置が中心でなくてよい）")]
    [SerializeField] Vector3 circleCenterOffset = Vector3.zero;

    [Header("【Chase】プレイヤーを見つける距離")]
    [SerializeField] float detectRange = 0f;

    [Header("【Chase】追いかける速さ")]
    [SerializeField] float chaseSpeed = 0f;

    // 出現したときの位置（ここを基準に動く）
    Vector3 startPos;

    // 円運動の今の角度
    float circleAngle = 0f;

    // 追いかける相手
    Transform player;

    void Start()
    {
        startPos = transform.position;

        // Player タグのついたオブジェクトを探す
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
        else if (pattern == MovePattern.Chase)
        {
            Debug.LogError("[EnemyMove] Player タグのオブジェクトが見つかりません。Player のタグが Player になっているか確認してください");
        }
    }

    void Update()
    {
        // パターンごとに動きを変える（第13回：enum ＋ switch文）
        switch (pattern)
        {
            case MovePattern.Spin:
                MoveSpin();
                break;

            case MovePattern.LeftRight:
                MoveLeftRight();
                break;

            case MovePattern.Circle:
                MoveCircle();
                break;

            case MovePattern.Chase:
                MoveChase();
                break;
        }
    }

    /// <summary>その場でくるくる回る</summary>
    void MoveSpin()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    /// <summary>左右に行ったり来たりする</summary>
    void MoveLeftRight()
    {
        // Mathf.Sin は -1 ～ 1 を行ったり来たりする関数。
        // これに幅をかけると、左右にゆれる動きになる。
        float x = Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = startPos + new Vector3(x, 0f, 0f);
    }

    /// <summary>円を描いて回る</summary>
    void MoveCircle()
    {
        // 角度を少しずつ増やしていく
        circleAngle += circleSpeed * Time.deltaTime;

        // 角度から円周上の位置を求める（Cos が横、Sin が奥行き）
        float rad = circleAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * circleRadius;
        float z = Mathf.Sin(rad) * circleRadius;

        transform.position = startPos + circleCenterOffset + new Vector3(x, 0f, z);
    }

    /// <summary>プレイヤーが近づいたら追いかける</summary>
    void MoveChase()
    {
        if (player == null) return;

        // プレイヤーとの距離を測る（第9回）
        float distance = Vector3.Distance(transform.position, player.position);

        // 見つける距離より近ければ追いかける
        if (distance <= detectRange)
        {
            // プレイヤーの方を向く（高さは合わせず、水平だけ向く）
            Vector3 target = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(target);

            // 向いた方向に進む
            transform.Translate(Vector3.forward * chaseSpeed * Time.deltaTime);
        }
    }
}
