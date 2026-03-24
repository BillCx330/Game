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

    //  强制精准吸附：直接跳到正确位置，绝不停在原地
    public void SnapToCorrectPosition()
    {
        if (isCorrect) return;

        // 【关键】直接赋值正确位置+正确旋转，100%对齐
        transform.position = correctPosition; 
        transform.rotation = Quaternion.Euler(0, 0, correctRotationZ);
        
        isCorrect = true;
        Debug.Log($"【精准吸附】{gameObject.name} 已完全贴合正确位置");

        // 通知游戏管理器
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