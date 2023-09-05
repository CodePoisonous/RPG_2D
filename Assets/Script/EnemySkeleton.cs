using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySkeleton : Entity
{
    private bool isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player Detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask PlayerMask;
    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 2.0f * facingDir, rb.velocity.y);
                Debug.Log("I see the player");
                //isAttacking = false;
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed * 2.0f * facingDir, rb.velocity.y);
                Debug.Log("Attacking! " + isPlayerDetected.collider.gameObject.name);
                //isAttacking = true;
            }
        }
        else 
        {
            Movement();
        }

        if (!isGrounded || isWallDetected) Flip();

    }

    private void Movement()
    {
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(transform.position,
            Vector2.right, playerCheckDistance * facingDir, PlayerMask);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position,
           new Vector3(transform.position.x + playerCheckDistance * facingDir,
           transform.position.y));
    }
}
