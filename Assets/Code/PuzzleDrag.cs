using UnityEngine;

public class PuzzleDrag : MonoBehaviour
{
    private PuzzlePiece _puzzle;
    private bool _isDrag;
    private Vector3 _offset;
    private float _zPos;
    private readonly float rotateAngle = 90f;

    void Awake()
    {
        _puzzle = GetComponent<PuzzlePiece>();
        _zPos = transform.position.z;
    }

    void OnMouseDown()
    {
        if (_puzzle.isCorrect) return;
        
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = _zPos;
        _offset = transform.position - mouseWorld;
        _isDrag = true;
        _puzzle.SetSortingOrder(4); // 选中时设为4
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
            _puzzle.SnapToCorrectPosition(); // 拼好自动设为2
        }
        else
        {
            _puzzle.SetSortingOrder(3); // 未拼好恢复为3
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !_puzzle.isCorrect)
        {
            transform.Rotate(0, 0, -rotateAngle, Space.World);
            Debug.Log($"{gameObject.name} 旋转90度，当前角度：{transform.eulerAngles.z}");
        }
    }
}