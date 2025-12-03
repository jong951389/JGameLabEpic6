using UnityEngine;
using UnityEngine.EventSystems;

public class FieldItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("데이터 연결")]
    public ItemData itemData; // ★ 이 아이템이 무엇인지 정의하는 데이터

    private Vector3 startPos;
    private Camera mainCamera;
    private Collider2D myCollider;

    private void Start()
    {
        mainCamera = Camera.main;
        myCollider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
        // 드래그 중에는 콜라이더를 꺼서 마우스가 뒤에 있는 UI(슬롯)를 감지하게 함
        if (myCollider != null) myCollider.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 인벤토리에 들어가지 못했다면 제자리 복귀 (들어갔다면 Destroy 될 것임)
        if (this != null)
        {
            if (myCollider != null) myCollider.enabled = true;
            transform.position = startPos;
        }
    }
}