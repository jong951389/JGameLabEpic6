using UnityEngine;

[CreateAssetMenu(fileName = "Magazine", menuName = "Scriptable Objects/Magazine")]
public class MagazineSO : ScriptableObject
{
    public string magazineName; // 탄창 이름
    public Sprite magazineIcon;     // 탄창 이름
    public float magazineWeight;    // 탄창 무게
    public float magazineDamage;     // 탄창 데미지
    public int magazineCapacity;    // 탄창 용량
    public string magazineExplanation; // 탄창 설명
}
