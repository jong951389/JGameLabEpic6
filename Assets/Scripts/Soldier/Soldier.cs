using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("장착 설정")]
    [SerializeField] private Transform weaponHolder; // 총기 장착할 위치
    [SerializeField] private GameObject initialWeaponPrefab; // 처음에 들고 시작할 무기 프리팹

    // 현재 들고 있는 무기의 스크립트 정보를 저장
    private Weapon currentWeaponHandler;

    private void Start()
    {
        // 게임 시작 시 기본 무기 장착
        if (initialWeaponPrefab != null)
        {
            EquipWeapon(initialWeaponPrefab);
        }
    }

    // 외부(인벤토리 등)에서 호출하여 무기를 교체하는 함수
    public void EquipWeapon(GameObject weaponPrefab)
    {
        // 1. 이미 들고 있는 무기가 있다면 제거 (또는 인벤토리로 되돌리기)
        if (currentWeaponHandler != null)
        {
            Destroy(currentWeaponHandler.gameObject);
        }

        // 2. 새 무기 생성 (Instantiate)
        // 일단 위치는 상관없이 생성하고, 나중에 Equip 함수로 위치를 잡습니다.
        GameObject newWeapon = Instantiate(weaponPrefab);

        // 3. 무기 스크립트 가져오기
        currentWeaponHandler = newWeapon.GetComponent<Weapon>();

        if (currentWeaponHandler != null)
        {
            // 4. 무기에게 "너 이제 내 손(weaponHolder)에 장착돼라"고 명령
            currentWeaponHandler.Equip(weaponHolder);
        }
    }
}