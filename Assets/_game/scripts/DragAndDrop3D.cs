using UnityEngine;

public class DragAndDrop3DRaycast : MonoBehaviour
{
    public Vector3 offset;
    public float initialY;
    public bool isDragging = false;
    public Camera mainCamera;
    public Transform selectedObject;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Xử lý khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast với layer "Box" để chọn object
            int layerMask = LayerMask.GetMask("Box");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                selectedObject = hit.transform;
                initialY = selectedObject.position.y;

                // Tính offset
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(selectedObject.position).z));
                offset = selectedObject.position - mousePosition;
                isDragging = true;
                selectedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        // Xử lý kéo
        if (isDragging && Input.GetMouseButton(0))
        {
            if (selectedObject != null)
            {
                // Bắn raycast vào layer "Plane"
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = LayerMask.GetMask("Plane");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    // Lấy điểm tiếp xúc và thêm 4 đơn vị theo trục Y
                    Vector3 newPosition = hit.point;
                    newPosition.y += 2f;

                    // Cập nhật vị trí và xoay
                    selectedObject.position = newPosition;
                    selectedObject.rotation = Quaternion.identity;
                }
            }
        }

        // Thả chuột
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if(selectedObject) selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            selectedObject = null;
        }
    }
}