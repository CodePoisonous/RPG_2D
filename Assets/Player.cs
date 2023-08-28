using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;
    private Animator anim = null;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;

    //private int facingDir = 1;
    private bool isFaceingRight = true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();
        checkInput();
        CollisionChecks();
        FlipCotroller();
        AnimatorContollers();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, 
            new Vector3(transform.position.x,
            transform.position.y - groundCheckDistance));
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void checkInput()
    {
        xInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) 
            && isGrounded) Jump();
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
        //facingDir *= -1;
        isFaceingRight = !isFaceingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AnimatorContollers()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", rb.velocity.x != 0);
        anim.SetBool("isGrounded", isGrounded);
    }

}
