using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private bool playerControlsEnabled = true;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        onFoot.Jump.performed += ctx => motor.Jump();
        look = GetComponent<PlayerLook>();
    }

    void FixedUpdate()
    {
        if (!playerControlsEnabled)
        {
            return;
        }

        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        if (!playerControlsEnabled)
        {
            return;
        }

        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        if (playerControlsEnabled)
        {
            onFoot.Enable();
        }
    }

    void OnDisable()
    {
        onFoot.Disable();
    }

    public void SetPlayerControlsEnabled(bool enabled)
    {
        playerControlsEnabled = enabled;

        if (enabled)
        {
            onFoot.Enable();
        }
        else
        {
            onFoot.Disable();
        }
    }
}