using UnityEngine;


public class Mousecontrol : MonoBehaviour
{

    private GameObject draggedObject;

    private Vector3 offset;

    private readonly float rotateAngle = 90f;

    private Camera mainCamera;

    void Awake()
    {

        mainCamera = Camera.main;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            SelectPuzzleObject();
        }

 
        if (Input.GetMouseButton(0) && draggedObject != null)
        {
            DragPuzzleObject();
        }


        if (Input.GetMouseButtonUp(0))
        {
            draggedObject = null;
        }


        if (Input.GetMouseButtonDown(1) && draggedObject != null)
        {
            RotatePuzzleObject();
        }
    }


    void SelectPuzzleObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            if (hit.collider.gameObject.CompareTag("puzzle"))
            {
                draggedObject = hit.collider.gameObject;

                Vector3 mouseWorldPos = GetMouseWorldPosition();
                offset = draggedObject.transform.position - mouseWorldPos;

                draggedObject.transform.position = new Vector3(
                    draggedObject.transform.position.x,
                    draggedObject.transform.position.y,
                    mouseWorldPos.z
                );
            }
        }
    }


    void DragPuzzleObject()
    {
        Vector3 targetPos = GetMouseWorldPosition() + offset;

        targetPos.z = draggedObject.transform.position.z;
        draggedObject.transform.position = targetPos;
    }


    void RotatePuzzleObject()
    {

        draggedObject.transform.Rotate(0, 0, -rotateAngle, Space.World);
        Debug.Log($"{draggedObject.name} 绕Z轴顺时针旋转90度，当前角度：{draggedObject.transform.eulerAngles.z}");
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;

        if (draggedObject != null)
        {
            mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - draggedObject.transform.position.z);
        }
        else
        {

            mouseScreenPos.z = 10f;
        }
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}