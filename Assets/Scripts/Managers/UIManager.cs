using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject Inventory;

    private void Awake()
    {
        Inventory.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(Inventory.activeSelf == false)
            {
                Inventory.SetActive(true);
                // 인벤토리 열릴 때 필요한 추가 작업들 여기에 작성
            }
            else
            {
                Inventory.SetActive(false);
                // 인벤토리 닫힐 때 필요한 추가 작업들 여기에 작성
            }
        }
    }
}
