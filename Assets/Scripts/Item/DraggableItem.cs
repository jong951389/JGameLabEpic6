using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("데이터")]
    public ItemData itemData; // ★ 핵심: 나는 어떤 아이템인가?

    private Image image;
    private Transform parentAfterDrag;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // 슬롯이 이 함수를 호출해서 데이터를 넣어줍니다.
    public void Initialize(ItemData data)
    {
        itemData = data;
        image.sprite = data.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // 제일 위로 올려서 보이게 함
        transform.SetAsLastSibling();

        image.raycastTarget = false; // 마우스가 통과해서 뒤에 있는 병사를 감지하게 함
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        canvasGroup.blocksRaycasts = true;

        // 갈 곳이 없으면(드롭 실패) 원래 슬롯으로 복귀
        if (transform.parent == transform.root)
        {
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
        }
    }
}