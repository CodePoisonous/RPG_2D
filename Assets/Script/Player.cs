using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;
    private Animator anim = null;

    private float xInput = 0;
    private int facingDir = 1;
    private bool isFaceingRight = true;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime = 0;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer = 0;

    [Header("Attack info")]
    [SerializeField] private float comboTime;
    private float comboTimeWindow = 0;
    private bool isAttaching = false;
    private int comboCounter = 0;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckInput();
        Movement();
        CollisionChecks();
        FlipCotroller();

        if (dashCooldownTimer >= 0) dashCooldownTimer -= Time.deltaTime;
        if (dashTime >= 0) dashTime -= Time.deltaTime;
        if(comboTimeWindow >= 0) comboTimeWindow -= Time.deltaTime;

        AnimatorContollers();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AttachOver()
    {
        isAttaching = false;
        ++comboCounter;
        if(comboCounter > 2) comboCounter = 0;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, 
            new Vector3(transform.position.x,
            transform.position.y - groundCheckDistance));
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Movement()
    {
        if (isAttaching)
            rb.velocity = new Vector2(0, 0);
        else if (dashTime > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        else
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartAttachEvent();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            DashAbility();

        if (Input.GetKeyDown(KeyCode.Space) 
            && isGrounded) Jump();
    }

    private void StartAttachEvent()
    {
        if (!isGrounded) return;

        if (comboTimeWindow < 0) comboCounter = 0;

        isAttaching = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0
            && !isAttaching)
        {
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CollisionChecks()
    {
        // 向场景中的碰撞体投射射线。
        isGrounded = Physics2D.Raycast(transform.position, 
            Vector2.down, groundCheckDistance, groundMask);
    }    

    private void FlipCotroller()
    {
        if ((rb.velocityX > 0 && !isFaceingRight)
            || (rb.velocityX < 0 && isFaceingRight))
            Flip();
    }

    private void Flip()
    {
        facingDir *= -1;
        isFaceingRight = !isFaceingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AnimatorContollers()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", rb.velocity.x != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttaching", isAttaching);
        anim.SetInteger("comboCounter", comboCounter);
    }
}
