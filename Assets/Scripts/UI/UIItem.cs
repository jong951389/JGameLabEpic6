using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("데이터")]
    public ItemData itemData; // 현재 내가 보여주고 있는 아이템 데이터

    private Transform parentAfterDrag;
    private Image image;
    private CanvasGroup canvasGroup;
    private Camera mainCamera;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        mainCamera = Camera.main;
    }

    // 데이터를 받아서 초기화하는 함수 (슬롯이 호출함)
    public void Initialize(ItemData data)
    {
        itemData = data;
        image.sprite = data.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // 최상위로 올려서 가려짐 방지
        transform.SetAsLastSibling();
        image.raycastTarget = false;
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

        // 1. UI 위가 아니라면 (땅바닥이라면) -> 필드 아이템 생성
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            SpawnToField();
            Destroy(gameObject); // 나(UI)는 삭제
            return;
        }

        // 2. 다른 슬롯에 못 들어갔다면 원래 자리로
        if (transform.parent == transform.root)
        {
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
        }
    }

    void SpawnToField()
    {
        if (itemData != null && itemData.prefab != null)
        {
            Vector3 dropPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            dropPos.z = 0;

            // 1. 생성된 오브젝트를 변수(spawnedObject)에 담습니다.
            GameObject spawnedObject = Instantiate(itemData.prefab, dropPos, Quaternion.identity);

            // 2. 생성된 오브젝트에서 MagazineManager 컴포넌트를 찾습니다.
            MagazineManager manager = spawnedObject.GetComponent<MagazineManager>();

            // 3. 매니저가 있다면 데이터를 넘겨줍니다.
            if (manager != null)
            {
                manager.SetData(itemData); // 이 함수를 호출해서 데이터를 심어줌
            }
        }
    }
}