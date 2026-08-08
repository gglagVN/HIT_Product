using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    private bool canMove = true;

    [Header("Movement")]
    public float speed = 5f;
    public float sprintSpeed = 8f;

    [Header("Gravity")]
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetMovementEnabled(bool value)
    {
        canMove = value;

        // Nếu khóa movement thì dừng vận tốc ngang.
        if (!canMove)
        {
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;
        }
    }

    public void ProcessMove(Vector2 input)
    {
        isGrounded = controller.isGrounded;

        // Không cho người chơi điều khiển di chuyển
        if (!canMove)
        {
            ApplyGravity();
            return;
        }

        Vector3 moveDirection = Vector3.zero;

        moveDirection.x = input.x;
        moveDirection.z = input.y;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift)
            ? sprintSpeed
            : speed;

        controller.Move(
            transform.TransformDirection(moveDirection)
            * currentSpeed
            * Time.deltaTime
        );

        ApplyGravity();
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (!canMove)
            return;

        if (controller.isGrounded)
        {
            playerVelocity.y =
                Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}