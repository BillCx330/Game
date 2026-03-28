using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PuzzlePiece : MonoBehaviour
{
    [Header("拼图配置")]
    public Vector3 correctPosition;          // 绝对正确位置
    public float correctRotationZ;           // 正确旋转
    public float snapDistance = 0.5f;        // 触发吸附的距离
    public float rotationTolerance = 5f;     // 旋转容错

    [Header("状态")]
    [HideInInspector] public bool isCorrect = false;

    private SpriteRenderer _spriteRenderer; // 新增：获取2D渲染组件

    void Awake()
    {
        // 初始化SpriteRenderer（必选，2D拼图需挂载该组件）
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"[{gameObject.name}] 缺少SpriteRenderer组件！");
        }
    }

    void Start()
    {
        // 初始化层级：未拼好=3，拼好=2
        SetSortingOrder(isCorrect ? 2 : 3);
    }

    // 新增：统一设置层级的方法
    public void SetSortingOrder(int order)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sortingOrder = order;
        }
    }

    // 检查是否满足吸附条件
    public bool CheckIfCorrect()
    {
        float positionDiff = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(correctPosition.x, correctPosition.y)
        );
        float rotationDiff = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, correctRotationZ));
        return (positionDiff < snapDistance) && (rotationDiff < rotationTolerance);
    }

    // 强制精准吸附：拼好后层级设为2
    public void SnapToCorrectPosition()
    {
        if (isCorrect) return;

        transform.position = correctPosition; 
        transform.rotation = Quaternion.Euler(0, 0, correctRotationZ);
        
        isCorrect = true;
        SetSortingOrder(2); // 拼好后层级=2
        Debug.Log($"【精准吸附】{gameObject.name} 已完全贴合正确位置");

        if (PuzzleGameManager.Instance != null)
            PuzzleGameManager.Instance.OnPieceCorrect(this);
    }

    [ContextMenu("当前位置设为正确位置")]
    public void SetCurrentPosAsCorrect()
    {
        correctPosition = transform.position;
        correctRotationZ = transform.eulerAngles.z;
    }
}