using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator), typeof(InputHandler))]
public class Player : MonoBehaviour
{
    #region Player Settings
    [SerializeField] private int _health;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 4;

    [Header("Jump")]
    [SerializeField] private float _jumpVelocity = 8;
    [SerializeField, Range(0.1f, 1f)] private float _variableJumpMultiplier = 0.5f;
    [SerializeField] private float _coyoteTime = 0.2f;
    [SerializeField] private int _amountJumpsInAir = 1;
    [SerializeField] private float _wallSlideVelocity = -3;
    [SerializeField] private float _gravity = 20;
    [SerializeField] private ParticleSystem _landingParticles;

    [Header("Checkers")]
    [SerializeField] private Transform _headBumpChecker;
    [SerializeField] private float _headBumpCheckerRadius = 0.3f;
    [SerializeField] private float _headBumpRebound = 1f;
    [SerializeField] private LayerMask _headBumpLayer;
    [Space]
    [SerializeField] private Transform _wallTouchedChecker;
    [SerializeField] private float _wallTouchedCheckerRadius = 0.3f;
    [SerializeField] private LayerMask _wallTouchedLayer;

    public float MoveSpeed => _moveSpeed;
    public float JumpVelocity => _jumpVelocity;
    public float CoyoteTime => _coyoteTime;
    public float VaribleJumpMultiplier => _variableJumpMultiplier;
    public int AmountJumpsInAir => _amountJumpsInAir;
    public bool IsGrounded => _controller.isGrounded;
    public float WallSlideVelocity => _wallSlideVelocity; 
    public float Gravity => _gravity;
    public Vector3 CurrentVelocity => _velocity;
    public Vector3 LookDirection => transform.forward;
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public InputHandler Input { get; private set; }

    private CharacterController _controller;
    #endregion

    #region Private fields

    private Vector3 _velocity;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Input = GetComponent<InputHandler>();
        _controller = GetComponent<CharacterController>();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, "move");
        JumpState = new PlayerJumpState(this, StateMachine, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, "inAir");
        LandState = new PlayerLandState(this, StateMachine, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "wallSlide");

    }
    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

        CheckIfHeadBumped();

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        _velocity.x = velocity;

        TryInvert(velocity);
    }

    public void SetVelosityY(float velocity) => _velocity.y = velocity;

    public void SetGravity(float gravity) => _gravity = gravity;

    public void PlayLandingParticles() => _landingParticles.Play();

    #endregion

    #region PlayerActions
  
    #endregion
    #region Checks
    public void CheckIfHeadBumped()
    {
        bool headBumped = Physics.CheckSphere(_headBumpChecker.position, _headBumpCheckerRadius, _headBumpLayer);
        if (headBumped)
            _velocity.y -= _headBumpRebound;
    }

    public bool CheckIfTouchedWall()
    {
        return Physics.CheckSphere(_wallTouchedChecker.position, _wallTouchedCheckerRadius, _wallTouchedLayer);
    }
    #endregion

    #region Others

    private void AnimtionTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void TryInvert(float newVelosity)
    {
        if (newVelosity < 0 && transform.forward != Vector3.left 
            || newVelosity > 0 && transform.forward != Vector3.right)
            transform.Rotate(Vector3.up, 180);
    }
    #endregion
}