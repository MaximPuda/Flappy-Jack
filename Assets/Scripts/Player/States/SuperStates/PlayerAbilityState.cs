public class PlayerAbilityState : PlayerState
{
    protected bool IsAbilityDone;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        IsAbilityDone = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(IsAbilityDone)
        {
            if (Player.IsGrounded && Player.CurrentVelocity.y < 0.01f)
                StateMachine.ChangeState(Player.IdleState);
            else
                StateMachine.ChangeState(Player.InAirState);
        }
    }
}
