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
        _puzzle.SetSortingOrder(4);
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
        else
        {
            _puzzle.SetSortingOrder(3);
        }
    }

    void OnMouseOver()
    {
        // 只有鼠标正下方最顶层的拼图块才能响应右键旋转
        if (Input.GetMouseButtonDown(1) && !_puzzle.isCorrect && IsTopmostObject())
        {
            transform.Rotate(0, 0, -rotateAngle, Space.World);
            Debug.Log($"{gameObject.name} 旋转90度，当前角度：{transform.eulerAngles.z}");
        }
    }

    /// <summary>
    /// 判断当前物体是否是鼠标下所有碰撞器中 sortingOrder 最高的（即最顶层）
    /// </summary>
    private bool IsTopmostObject()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        if (hits.Length == 0) return false;

        // 按 sortingOrder 降序排序，如果相同则按 Z 轴降序（Z 越小越靠前）
        System.Array.Sort(hits, (a, b) =>
        {
            SpriteRenderer srA = a.collider.GetComponent<SpriteRenderer>();
            SpriteRenderer srB = b.collider.GetComponent<SpriteRenderer>();
            int orderA = srA != null ? srA.sortingOrder : -1;
            int orderB = srB != null ? srB.sortingOrder : -1;
            if (orderA != orderB) return orderB.CompareTo(orderA);
            // 排序相同时，Z 值越小（越靠近相机）的越靠前
            return a.transform.position.z.CompareTo(b.transform.position.z);
        });

        return hits[0].collider.gameObject == gameObject;
    }
}