using UnityEngine;

/// <summary>
/// ステージの一番奥にあるゴール。
/// ふだんは動いていない（触れても何も起きない）。
/// ボスを倒すと有効になり、プレイヤーが触れるとゲームクリア。
/// 【第6回】OnTriggerEnter / タグ判定　【第13回】シーン遷移
/// </summary>
public class GoalPoint : MonoBehaviour
{
    [Header("有効になる前の見た目（暗いマテリアル）")]
    [SerializeField] Material inactiveMaterial;

    [Header("有効になったあとの見た目（光るマテリアル）")]
    [SerializeField] Material activeMaterial;

    // ゴールが使えるようになっているか
    bool isActive = false;

    static GoalPoint instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isActive = false;
        ApplyMaterial();
    }

    void OnTriggerEnter(Collider other)
    {
        // まだ有効になっていないゴールは反応しない
        if (!isActive) return;

        if (other.tag == "Player")
        {
            Debug.Log("ゴール！ ゲームクリア！");
            GameManager.GameClear();
        }
    }

    /// <summary>ゴールを有効にする（ボス撃破時に呼ばれる）</summary>
    public static void ActivateGoal()
    {
        if (instance == null)
        {
            Debug.LogError("[GoalPoint] シーンに GoalPoint がありません。ゴールを配置してください");
            return;
        }

        instance.isActive = true;
        instance.ApplyMaterial();
    }

    /// <summary>状態に合わせて見た目を変える</summary>
    void ApplyMaterial()
    {
        Renderer r = GetComponent<Renderer>();
        if (r == null) return;

        if (isActive && activeMaterial != null)
        {
            r.material = activeMaterial;
        }
        else if (!isActive && inactiveMaterial != null)
        {
            r.material = inactiveMaterial;
        }
    }
}
