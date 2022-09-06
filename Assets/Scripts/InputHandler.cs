using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.started += context => OnJumpInputStart();
        _input.Player.Jump.canceled += context => OnJumpInputCanceled();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        MoveInput = _input.Player.Move.ReadValue<Vector2>();
    }

    private void OnJumpInputStart()
    {
        JumpInput = true;
        JumpInputStop = false;
    }

    private void OnJumpInputCanceled()
    { 
        JumpInputStop = true;
    }

    public void SetJumpInputFalse() => JumpInput = false;
}
