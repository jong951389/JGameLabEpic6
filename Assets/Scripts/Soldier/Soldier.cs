using UnityEngine;
using UnityEngine.EventSystems;

public class Soldier : MonoBehaviour, IDropHandler
{
    [Header("필수 연결")]
    [SerializeField] private Transform weaponHolder; // 무기가 장착될 손 위치

    // ★ 중요: 병사 자식에 있는 'MagazineRack'을 여기에 연결해줘야 합니다.
    // 그래야 무기가 생성될 때 "너의 탄창통은 이거야"라고 알려줄 수 있습니다.
    [SerializeField] private MagazineManager myMagazineRack;

    private Weapon currentWeapon;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        if (droppedObj.TryGetComponent<UIItem>(out UIItem uiItem))
        {
            if (uiItem.itemData is WeaponSO weaponData)
            {
                EquipWeapon(weaponData);
                Destroy(droppedObj); // 인벤토리 아이콘 삭제
            }
        }
    }

    public void EquipWeapon(WeaponSO data)
    {
        // 1. 기존 무기 파괴
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        // 2. 새 무기 생성 (생성하면서 바로 weaponHolder를 부모로 설정)
        if (data.prefab != null)
        {
            // Instantiate(프리팹, 부모트랜스폼) -> 이렇게 하면 자동으로 자식으로 들어갑니다.
            GameObject newWeaponObj = Instantiate(data.prefab, weaponHolder);

            // 3. 위치 및 회전 초기화 (삭제된 Equip 메서드가 하던 일)
            newWeaponObj.transform.localPosition = Vector3.zero;
            newWeaponObj.transform.localRotation = Quaternion.Euler(0, 0, 90);

            // 4. Weapon 컴포넌트 설정
            currentWeapon = newWeaponObj.GetComponent<Weapon>();

            if (currentWeapon != null)
            {
                // 데이터 주입
                currentWeapon.weaponData = data;

                // ★★★ 핵심 수정: Equip 메서드 호출 대신 직접 매니저 연결 ★★★
                // 프리팹 상태의 무기는 씬에 있는 MagazineRack을 모르기 때문에
                // 병사가 직접 "이게 니 매니저야"라고 꽂아줘야 합니다.
                currentWeapon.magazineManager = myMagazineRack;
            }
        }
    }
}