using UnityEngine;

public class PuzzleDrag : MonoBehaviour
{
    private PuzzlePiece _puzzle;
    private bool _isDrag;
    private Vector3 _offset;
    private float _zPos;
    private readonly float rotateAngle = 90f; // 旋转角度

    void Awake()
    {
        _puzzle = GetComponent<PuzzlePiece>();
        _zPos = transform.position.z; // 固定Z轴，防止错位
    }

    void OnMouseDown()
    {
        if (_puzzle.isCorrect) return;
        
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = _zPos;
        _offset = transform.position - mouseWorld;
        _isDrag = true;
    }

    void OnMouseDrag()
    {
        if (!_isDrag || _puzzle.isCorrect) return;
        
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = _zPos;
        transform.position = mouseWorld + _offset;
    }

    void OnMouseUp()
    {
        _isDrag = false;
        if (_puzzle.isCorrect) return;

        if (_puzzle.CheckIfCorrect())
        {
            _puzzle.SnapToCorrectPosition();
        }
    }

    // 新增：右键旋转（通过OnMouseOver检测鼠标悬浮+右键点击）
    void OnMouseOver()
    {
        // 只在鼠标悬浮在拼图上、且拼图未拼对时，响应右键
        if (Input.GetMouseButtonDown(1) && !_puzzle.isCorrect)
        {
            transform.Rotate(0, 0, -rotateAngle, Space.World);
            Debug.Log($"{gameObject.name} 旋转90度，当前角度：{transform.eulerAngles.z}");
        }
    }
}