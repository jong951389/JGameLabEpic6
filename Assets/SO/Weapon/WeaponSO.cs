using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ItemData
{
    [Header("총기 성능")]
    public float fireRate;  // 공격 속도
    public float range;     // 사거리
}
