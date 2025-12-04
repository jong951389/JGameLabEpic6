using UnityEngine;

public class Magazine : MonoBehaviour
{
    [Header("데이터 연결")]
    public MagazineSO data; // 인스펙터에서 SO 파일 연결

    [Header("런타임 상태")]
    public int currentAmmo; // 현재 남은 총알

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 게임 시작 시 데이터대로 초기화
        Initialize();
    }

    public void Initialize()
    {
        if (data != null)
        {
            currentAmmo = data.magazineCapacity;

            if (spriteRenderer != null && data.icon != null)
                spriteRenderer.sprite = data.icon;

            gameObject.name = data.itemName;
        }
    }

    // 총알 1발 소모 시도
    public bool ConsumeAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            return true;
        }
        return false;
    }

    // 무기에게 총알 프리팹 정보를 넘겨주는 함수
    public GameObject GetBulletPrefab()
    {
        return data != null ? data.bulletPrefab : null;
    }
}