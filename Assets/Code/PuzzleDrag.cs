using UnityEngine;

public class PuzzleDrag : MonoBehaviour
{
    private PuzzlePiece _puzzle;
    private bool _isDrag;
    private Vector3 _offset;
    private float _zPos;

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

    // ✅ 松手触发：满足条件 → 直接吸附到正确位置
    void OnMouseUp()
    {
        _isDrag = false;
        if (_puzzle.isCorrect) return;

        // 距离够近 → 强制精准吸附，不留在当前位置
        if (_puzzle.CheckIfCorrect())
        {
            _puzzle.SnapToCorrectPosition();
        }
    }
}