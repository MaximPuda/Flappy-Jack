using UnityEngine;

public class PlayerState
{
    protected Player Player;
    protected PlayerStateMachine StateMachine;
    
    protected float StartTime;
    protected bool IsAnimationFinished;

    private string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        Player = player;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        StartTime = Time.time;
        Player.Anim.SetBool(_animBoolName, true);
        Debug.Log(_animBoolName);
        IsAnimationFinished = false;

    }
    public virtual void Exit()
    {
        Player.Anim.SetBool(_animBoolName, false);
    }

    public virtual void DoChecks() { }

    public virtual void LogicUpdate() 
    {
        DoChecks();
    }

    public virtual void PhysicsUpdate() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
}
