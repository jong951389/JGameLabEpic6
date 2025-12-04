using UnityEngine;

[CreateAssetMenu(fileName = "Magazine", menuName = "Scriptable Objects/Magazine")]
public class MagazineSO : ItemData
{
    public string magazineType; // 탄창 종류
    public int magazineCapacity; // 탄창 용량

    [Header("발사체 설정")]
    public GameObject bulletPrefab; 
}
