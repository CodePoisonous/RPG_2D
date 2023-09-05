using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : Entity
{
    private float xInput;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Attack Info")]
    [SerializeField] private float comboTime;
    private float comboTimeWindow;
    private bool isAttaching;
    private int comboCounter;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        CheckInput();
        Movement();
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

    private void FlipCotroller()
    {
        if ((rb.velocityX > 0 && !isFaceingRight)
            || (rb.velocityX < 0 && isFaceingRight))
            Flip();
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
