using UnityEngine;

[RequireComponent(typeof(Collider))] // 强制要求碰撞体，保证射线检测有效
public class PuzzlePiece : MonoBehaviour
{
    [Header("拼图配置")]
    [Tooltip("该拼图的正确世界坐标")]
    public Vector3 correctPosition;          // 正确位置
    [Tooltip("该拼图的正确旋转角度（绕Z轴）")]
    public float correctRotationZ;           // 正确旋转角度
    [Tooltip("吸附距离阈值（靠近多少自动吸附）")]
    public float snapDistance = 0.5f;        // 吸附距离（建议0.3-0.8）
    [Tooltip("旋转误差允许值（度）")]
    public float rotationTolerance = 5f;     // 旋转容错

    [Header("状态标识")]
    [HideInInspector] public bool isCorrect = false; // 是否拼对

    /// <summary>
    /// 检查当前位置和旋转是否正确
    /// </summary>
    /// <returns>是否拼对</returns>
    public bool CheckIfCorrect()
    {
        // 计算位置偏差（仅X/Y轴，忽略Z轴）
        float positionDiff = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(correctPosition.x, correctPosition.y)
        );

        // 计算旋转偏差（绕Z轴，处理360度循环）
        float rotationDiff = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, correctRotationZ));

        // 位置和旋转都符合要求才算拼对
        isCorrect = (positionDiff < snapDistance) && (rotationDiff < rotationTolerance);
        return isCorrect;
    }

    /// <summary>
    /// 吸附到正确位置并修正旋转
    /// </summary>
    public void SnapToCorrectPosition()
    {
        // 固定Z轴，仅修正X/Y位置和Z轴旋转
        transform.position = new Vector3(correctPosition.x, correctPosition.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, correctRotationZ);
        isCorrect = true;

        // 触发拼对事件（供其他脚本监听）
        PuzzleGameManager.Instance.OnPieceCorrect(this);
    }

    /// <summary>
    /// 快速赋值：将当前位置设为正确位置（编辑器调试用）
    /// </summary>
    [ContextMenu("当前位置设为正确位置")]
    public void SetCurrentPosAsCorrect()
    {
        correctPosition = transform.position;
        correctRotationZ = transform.eulerAngles.z;
        Debug.Log($"{gameObject.name} 已将当前位置设为正确位置");
    }
}