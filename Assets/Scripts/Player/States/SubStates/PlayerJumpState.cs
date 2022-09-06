using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _jumpsInAirLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        Player.SetVelosityY(Player.JumpVelocity);
        Player.Input.SetJumpInputFalse();
        Player.InAirState.SetIsJumping();
        Player.InAirState.EndCoyoteTime();
        IsAbilityDone = true;
    }

    public bool CanJumpInAir() => _jumpsInAirLeft > 0;

    public void ResetJumpsInAirLeft() => _jumpsInAirLeft = Player.AmountJumpsInAir;

    public void DecreaseJumpsInAirLeft() => _jumpsInAirLeft--;
    public void IncreaseJumpsInAirLeft() => _jumpsInAirLeft++;
}
