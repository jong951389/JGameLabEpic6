using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private UIItem currentItem; // 현재 슬롯에 있는 아이템

    public void OnDrop(PointerEventData eventData)
    {
        // 1. 이미 인벤토리에 있는 UIItem을 옮길 때
        if (eventData.pointerDrag.TryGetComponent<UIItem>(out UIItem draggingUI))
        {
            if (transform.childCount == 0) // 비어있으면
            {
                draggingUI.transform.SetParent(transform);
                draggingUI.transform.localPosition = Vector3.zero;
            }
            return;
        }

        // 2. 필드 아이템(FieldItem)을 드래그해서 넣을 때
        if (eventData.pointerDrag.TryGetComponent<FieldItem>(out FieldItem fieldItem))
        {
            if (transform.childCount == 0) // 비어있으면
            {
                // A. UI 아이템 생성 (코드로 즉석 생성)
                CreateUIItem(fieldItem.itemData);

                // B. 필드 아이템은 삭제 (0.1초 딜레이로 에러 방지)
                Destroy(fieldItem.gameObject, 0.1f);
            }
        }
    }

    // 데이터로 UI 만들기
    void CreateUIItem(ItemData data)
    {
        GameObject uiObj = new GameObject("UIItem");
        uiObj.transform.SetParent(transform);

        // RectTransform 설정 (꽉 채우기)
        RectTransform rect = uiObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        // 컴포넌트 추가
        uiObj.AddComponent<Image>();
        UIItem uiItemScript = uiObj.AddComponent<UIItem>();

        // 데이터 주입
        uiItemScript.Initialize(data);
    }
}