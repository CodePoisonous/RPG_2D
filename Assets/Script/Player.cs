using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] public float moveSpeed = 12f;
    [SerializeField] public float jumpForce = 12f;
    public int facingDir { get; private set; } = 1;
    public bool isFacingRight = true;

    [Header("Dash Info")]
    [SerializeField] public float dashSpeed = 25f;
    [SerializeField] public float dashDuration = 0.2f;
    public float dashDir { get; private set; } = 1f;

    [Header("Collision Info")]
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckDistance = 0.15f;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public float wallCheckDistance = 0.2f;
    [SerializeField] public LayerMask whatIsWall;


    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region states
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    #endregion


    // public interface
    ///////////////////////////////////////////////////////////////////
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGroundDetected()
        => Physics2D.Raycast(groundCheck.position, 
            Vector2.down, groundCheckDistance, whatIsGround);

    public void FlipController(float _x)
    {
        if (_x > 0 && !isFacingRight
            || _x < 0 && isFacingRight)
            Flip();
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    /// private interface
    /// ///////////////////////////////////////////////////////////////
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        CheckForDashInput(); // 为了能在各个状态中随意切换到冲刺状态，所以放在了player中进行按键响应
    }

    private void CheckForDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir != 0)
                stateMachine.ChangeState(dashState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
