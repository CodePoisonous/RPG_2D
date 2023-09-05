using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;

    protected int facingDir = 1;
    protected bool isFaceingRight = true;

    [Header("Collision Info Ground")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask groundMask;
    protected bool isGrounded;

    [Header("Collision Info Wall")]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    protected bool isWallDetected;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void CollisionChecks()
    {
        // 向场景中的碰撞体投射射线。
        isGrounded = Physics2D.Raycast(groundCheck.position,
            Vector2.down, groundCheckDistance, groundMask);

        isWallDetected = Physics2D.Raycast(wallCheck.position,
            Vector2.right, wallCheckDistance * facingDir, groundMask);
    }

    protected virtual void Flip()
    {
        facingDir *= -1;
        isFaceingRight = !isFaceingRight;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * facingDir,
            wallCheck.position.y));
    }
}
