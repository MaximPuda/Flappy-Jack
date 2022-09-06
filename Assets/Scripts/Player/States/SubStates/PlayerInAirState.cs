using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private Vector2 _moveInput;

    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _isJumping;
    private bool _isCoyoteTime;
    private bool _isTouchedWall;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        _moveInput = Player.Input.MoveInput;
        _jumpInput = Player.Input.JumpInput;
        _jumpInputStop = Player.Input.JumpInputStop;
        _isTouchedWall = Player.CheckIfTouchedWall();

        DoVariableJump();
        DoGravity();

        if (_jumpInput && _isCoyoteTime)
        {
            Player.Input.SetJumpInputFalse();
            StateMachine.ChangeState(Player.JumpState);
        }
        else if (_jumpInput && Player.JumpState.CanJumpInAir())
        {
            Player.Input.SetJumpInputFalse();
            Player.JumpState.DecreaseJumpsInAirLeft();
            StateMachine.ChangeState(Player.JumpState);
        }
        

        if (Player.IsGrounded && Player.CurrentVelocity.y < 0.01f)
            StateMachine.ChangeState(Player.LandState);
        else
        {
            Player.SetVelocityX(_moveInput.x * Player.MoveSpeed);

            Player.Anim.SetFloat("velocityY", Player.CurrentVelocity.y);
            Player.Anim.SetFloat("velocityX", Mathf.Abs( Player.CurrentVelocity.x));
        }

        if (Player.CurrentVelocity.y < 0 && _isTouchedWall && _moveInput.x != 0)
            StateMachine.ChangeState(Player.WallSlideState);
    }

    private void DoVariableJump()
    {
        if (_isJumping)
        {
            if (_jumpInputStop)
            {
                Player.SetVelosityY(Player.CurrentVelocity.y * Player.VaribleJumpMultiplier);
                _isJumping = false;
            }
            else if (Player.CurrentVelocity.y < 0)
                _isJumping = false;
        }
    }

    private void DoGravity()
    {
        if (!Player.IsGrounded)
            Player.SetVelosityY(Player.CurrentVelocity.y - Player.Gravity * Time.deltaTime);
    }

    private void CheckCoyoteTime()
    {
        if (Time.time > StartTime + Player.CoyoteTime)
            EndCoyoteTime();
    }

    public void StartCoyoteTime() => _isCoyoteTime = true;

    public void EndCoyoteTime() => _isCoyoteTime = false;

    public void SetIsJumping() => _isJumping = true;
}