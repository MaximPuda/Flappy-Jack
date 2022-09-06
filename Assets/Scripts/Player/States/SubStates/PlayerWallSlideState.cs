using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerAbilityState
{
    private Vector2 _moveInput;
    private bool _jumpImput;
    private bool _isTouchedWall;

    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName){ }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        Player.SetVelosityY(Player.WallSlideVelocity);
        Player.Anim.SetFloat("LookDirectionX", Player.LookDirection.x);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _isTouchedWall = Player.CheckIfTouchedWall();
        _moveInput = Player.Input.MoveInput;
        _jumpImput = Player.Input.JumpInput;

        if (!_isTouchedWall)
            StateMachine.ChangeState(Player.InAirState);
        else if (Player.IsGrounded)
            StateMachine.ChangeState(Player.LandState);
        else if(_jumpImput)
        {
            Player.Input.SetJumpInputFalse();
            Player.JumpState.DecreaseJumpsInAirLeft();
            StateMachine.ChangeState(Player.JumpState);
        }    
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
