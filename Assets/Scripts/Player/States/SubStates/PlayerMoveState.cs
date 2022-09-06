using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float velocityX = MoveInput.x * Player.MoveSpeed;
        Player.SetVelocityX(velocityX);

        Player.Anim.SetFloat("velocityX", Mathf.Abs(velocityX));

        if (MoveInput.x == 0)
            StateMachine.ChangeState(Player.IdleState);
    }
}
