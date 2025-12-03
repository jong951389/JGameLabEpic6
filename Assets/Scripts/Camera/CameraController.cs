using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField] float panSpeed = 20f;
    [SerializeField] float panBorderThickness = 10f;

    [Header("이동 제한 (오브젝트 연결)")]
    [Tooltip("카메라가 이 오브젝트보다 위로 올라가지 못합니다.")]
    public Transform topLimitObject;

    [Tooltip("카메라가 이 오브젝트보다 아래로 내려가지 못합니다.")]
    public Transform bottomLimitObject;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 pos = transform.position;

        // 1. 위쪽 이동
        if (mousePos.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }

        // 2. 아래쪽 이동
        if (mousePos.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }

        // 3. 오브젝트 위치를 기준으로 이동 제한 (Clamp)
        // 오브젝트가 연결되어 있을 때만 작동하도록 null 체크
        if (topLimitObject != null && bottomLimitObject != null)
        {
            // pos.y를 '아래쪽 오브젝트의 Y'와 '위쪽 오브젝트의 Y' 사이로 가둡니다.
            pos.y = Mathf.Clamp(pos.y, bottomLimitObject.position.y, topLimitObject.position.y);
        }

        transform.position = pos;
    }

    // 에디터 씬 뷰에서 제한 선을 눈으로 쉽게 보기 위한 기능 (게임 실행엔 영향 없음)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 위쪽 제한선 그리기
        if (topLimitObject != null)
        {
            Gizmos.DrawLine(new Vector3(-100, topLimitObject.position.y, 0), new Vector3(100, topLimitObject.position.y, 0));
        }

        // 아래쪽 제한선 그리기
        if (bottomLimitObject != null)
        {
            Gizmos.DrawLine(new Vector3(-100, bottomLimitObject.position.y, 0), new Vector3(100, bottomLimitObject.position.y, 0));
        }
    }
}