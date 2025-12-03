using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("데이터 연결")]
    [Tooltip("여기에 만들어둔 WeaponSO 파일을 드래그해서 넣으세요")]
    public WeaponSO weaponData;

    [Header("상태 확인")]
    public bool isEquipped = false; // 장착 여부 확인용 bool변수

    [Header("설정")]
    [SerializeField] private string enemyTag = "Enemy"; // 적 태그
    [SerializeField] private Transform pivotPoint;      // 총이 회전할 중심축 (없으면 본인 Transform 사용)
    [SerializeField] private Transform firePoint;       // 총알이 나갈 위치 (총구)

    // 내부 연산 변수
    private Transform currentTarget;
    private float fireCountdown = 0f;

    private void Start()
    {
        // 최적화를 위해 매 프레임 적을 찾는 대신 0.5초마다 적을 탐색w
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    private void Update()
    {
        // 장착되지 않았다면 아무것도 하지 않음 (공격 로직 중단)
        if (!isEquipped || weaponData == null) return;

        // 데이터가 없으면 작동 중지
        if (weaponData == null) return;

        // 쿨타임 감소
        if (fireCountdown > 0f)
        {
            fireCountdown -= Time.deltaTime;
        }

        // 타겟이 없거나 사거리 밖으로 나갔다면 타겟 해제
        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.position);
            if (distance > weaponData.range)
            {
                currentTarget = null;
            }
        }

        // 타겟이 있다면 공격 로직 수행
        if (currentTarget != null)
        {
            // 1. 적 바라보기 (회전)
            RotateTowardsTarget();

            // 2. 쿨타임이 찼다면 발사
            if (fireCountdown <= 0f)
            {
                Shoot();
                // 쿨타임 초기화 (fireRate가 0.2면 0.2초마다 발사)
                fireCountdown = weaponData.fireRate;
            }
        }
    }

    public void Equip(Transform parentHolder)
    {
        // 1. 장착 상태로 변경
        isEquipped = true;

        // 2. 부모 설정 (병사의 손으로 이동)
        transform.SetParent(parentHolder);

        // 3. 위치와 회전 초기화 (손 위치에 딱 붙게)
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Unequip()
    {
        isEquipped = false;
        transform.SetParent(null); // 부모 연결 해제
    }

    // 가장 가까운 적 찾기
    void UpdateTarget()
    {
        if (weaponData == null) return;

        // 태그를 가진 모든 오브젝트 검색 (나중에는 LayerMask로 최적화 권장)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            // 사거리 안에 있고, 지금까지 찾은 적보다 더 가깝다면 갱신
            if (distanceToEnemy <= weaponData.range && distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            currentTarget = nearestEnemy.transform;
        }
        else
        {
            currentTarget = null;
        }
    }

    // 적 방향으로 총 돌리기
    void RotateTowardsTarget()
    {
        Vector2 direction = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 피벗 포인트가 있으면 피벗을 돌리고, 없으면 자신을 돌림
        Transform targetTrans = pivotPoint != null ? pivotPoint : transform;
        targetTrans.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 발사 (지금은 로그만 출력)
    void Shoot()
    {
        // 나중에 여기에 총알 생성 코드나 레이캐스트 코드가 들어감
        Debug.Log($"<color=yellow>[{weaponData.itemName}]</color> 탕! (사거리: {weaponData.range})");
    }

    // 에디터에서 사거리 눈으로 확인하기 (선택 시에만 보임)
    private void OnDrawGizmosSelected()
    {
        if (weaponData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, weaponData.range);
        }
    }
}