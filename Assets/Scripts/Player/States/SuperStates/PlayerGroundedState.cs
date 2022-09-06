using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected Vector2 MoveInput;

    private bool _jumpInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        Player.Input.SetJumpInputFalse();
        Player.JumpState.ResetJumpsInAirLeft();
        Player.PlayLandingParticles();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        MoveInput = Player.Input.MoveInput;
        _jumpInput = Player.Input.JumpInput;

        if (_jumpInput)
            StateMachine.ChangeState(Player.JumpState);
        else if (!Player.IsGrounded)
        {
            Player.InAirState.StartCoyoteTime();
            StateMachine.ChangeState(Player.InAirState);
        }
    }
}
