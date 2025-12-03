using UnityEngine;
public enum ItemType
{
    Weapon,
    Magazine,
    // 나중을 위한 소비아이템 등 추가 아이템 종류는 여기 추가
}
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public Sprite icon;
    public float ItemWeight; // 아이템 무게
    public ItemType itemType; // 아이템 타입 구분
    [TextArea]
    public string Explanation; // 아이템 설명

    [Header("연결 정보 (가장 중요!)")]
    // ★ 인벤토리에서 밖으로 던질 때 생성될 '원본 프리팹'
    public GameObject prefab;
}
