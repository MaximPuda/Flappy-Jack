public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        //Player.SetVelosityY(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (MoveInput.x != 0)
            StateMachine.ChangeState(Player.MoveState);
        else if (IsAnimationFinished)
            StateMachine.ChangeState(Player.IdleState);
    }
}
