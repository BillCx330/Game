using UnityEngine;

public class Mousecontrol : MonoBehaviour
{
    private GameObject draggedObject; // 正在拖拽的拼图块
    private Vector3 offset;           // 鼠标与拼图的偏移量
    private readonly float rotateAngle = 90f; // 每次旋转90度
    private Camera mainCamera;        // 主相机引用

    void Awake()
    {
        mainCamera = Camera.main; // 初始化主相机
    }

    void Update()
    {
        // 左键按下：选中拼图
        if (Input.GetMouseButtonDown(0))
        {
            SelectPuzzleObject();
        }

        // 左键按住：拖拽拼图
        if (Input.GetMouseButton(0) && draggedObject != null)
        {
            DragPuzzleObject();
        }

        // 左键松开：取消拖拽
        if (Input.GetMouseButtonUp(0))
        {
            draggedObject = null;
        }

        // 右键按下：旋转拼图（核心新增/修复）
        if (Input.GetMouseButtonDown(1) && draggedObject != null)
        {
            RotatePuzzleObject();
        }
    }

    // 选中拼图块
    void SelectPuzzleObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 只选中带"puzzle"标签的物体
            if (hit.collider.gameObject.CompareTag("puzzle"))
            {
                draggedObject = hit.collider.gameObject;
                // 计算鼠标与拼图的偏移（防止点击瞬间瞬移）
                Vector3 mouseWorldPos = GetMouseWorldPosition();
                offset = draggedObject.transform.position - mouseWorldPos;
                // 固定Z轴，防止拖拽时错位
                draggedObject.transform.position = new Vector3(
                    draggedObject.transform.position.x,
                    draggedObject.transform.position.y,
                    mouseWorldPos.z
                );
            }
        }
    }

    // 拖拽拼图块
    void DragPuzzleObject()
    {
        Vector3 targetPos = GetMouseWorldPosition() + offset;
        targetPos.z = draggedObject.transform.position.z; // 固定Z轴
        draggedObject.transform.position = targetPos;
    }

    // 旋转拼图块（修复乱码+明确逻辑）
    void RotatePuzzleObject()
    {
        // 绕世界坐标系Z轴逆时针旋转90度（可改-90为90变成顺时针）
        draggedObject.transform.Rotate(0, 0, -rotateAngle, Space.World);
        // 打印旋转信息（方便调试，小白可删）
        Debug.Log($"{draggedObject.name} 绕Z轴旋转90度，当前角度：{draggedObject.transform.eulerAngles.z}");
    }

    // 获取鼠标的世界坐标（核心工具方法）
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        if (draggedObject != null)
        {
            // 计算相机到拼图的Z轴距离，防止坐标偏移
            mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - draggedObject.transform.position.z);
        }
        else
        {
            mouseScreenPos.z = 10f; // 默认Z轴距离
        }
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}