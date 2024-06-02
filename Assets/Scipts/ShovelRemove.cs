using UnityEngine;
using UnityEngine.EventSystems;

public class ShovelRemove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initialPosition;
    public LayerMask plantMask;
    private Gamemanager gameManager;

    void Start()
    {
        initialPosition = transform.position;
        gameManager = FindObjectOfType<Gamemanager>(); // Tìm đối tượng Gamemanager trong scene
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Bắt đầu kéo");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, plantMask);
        if (hit.collider)
        {
            Destroy(hit.collider.gameObject);
            Debug.Log("Đã xóa đối tượng");

            // Gọi phương thức NotifyPlantRemoved trong Gamemanager
            if (gameManager != null)
            {
                gameManager.NotifyPlantRemoved(hit.collider.transform.position);
            }
        }

        transform.position = initialPosition;
    }
}
