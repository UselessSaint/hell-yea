using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerMovementController pmc;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pmc = GetComponent<PlayerMovementController>();
    }

    void FixedUpdate() {
        if (rb.linearVelocityX!= 0.0f) {
            animator.SetBool("IsMoving", true);
        } else {
            animator.SetBool("IsMoving", false);
        }

        if (rb.linearVelocityX < 0.0f) {
            FlipSprite(Vector2.left);
        } else if (rb.linearVelocityX > 0.0f) {
            FlipSprite(Vector2.right);
        }

        if (rb.linearVelocityY > 0.0f) {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
        if (rb.linearVelocityY < 0.0f) {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }

        if (pmc.GetIsGrounded) {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }

    public void FlipSprite(Vector2 direction) {
        if (direction == Vector2.right) {
            sr.flipX = false;
        } else if (direction == Vector2.left) {
            sr.flipX = true;
        }
    }
}
