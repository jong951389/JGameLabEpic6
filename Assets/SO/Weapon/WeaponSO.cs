using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("기본 정보")]
    public string weaponName;       // 무기 이름 
    public Sprite weaponIcon;       // 무기 아이콘
    [TextArea]
    public string weaponExplanation; // 무기 설명

    [Header("총기 성능")]
    public float fireRate;  // 공격 속도
    public float range;     // 사거리
}
