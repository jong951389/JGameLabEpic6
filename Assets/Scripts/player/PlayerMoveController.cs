using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    [Header("플레이어 조작 변수")]
    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D rb;
    Vector2 moveInput;
    PlayerInput inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;

        // 물리 충돌로 인해 캐릭터가 팽이처럼 도는 것을 방지 (코드로 회전시키는 건 가능)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        inputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        PlayerMove();   
    }

    void PlayerMove()
    {
        // 1. 이동 처리
        rb.linearVelocity = moveInput * moveSpeed;

        // 2. 회전 처리 (입력이 있을 때만 회전)
        if (moveInput.sqrMagnitude > 0.01f) // 아주 미세한 입력은 무시
        {
            // 아크탄젠트(Atan2)를 이용해 벡터(x, y)를 각도(라디안)로 변환 후, 도(Degree)로 변경
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            // 스프라이트가 기본적으로 어디를 보고 있는지에 따라 보정값(-90)이 필요할 수 있습니다.
            // 유니티 2D 스프라이트는 보통 위쪽(Up)을 보고 그려지는데, 수학적 0도는 오른쪽(Right)이라서 90도를 빼줍니다.
            // 만약 스프라이트가 오른쪽을 보고 있다면 "- 90f" 부분을 지우세요.
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}