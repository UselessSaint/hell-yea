using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D cl;
    private InputSystem_Actions inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector2 colliderSize;

    [Header("Movement variables")]
    [SerializeField]
    private Vector2 inputDirection;
    [SerializeField]
    private float defaultPlayerSpeed = 10.0f;
    [SerializeField]
    private float playerAcceleration = 2.0f;
    [SerializeField]
    private float inAirPlayerAccelerationMultiplier = 0.25f;
    [SerializeField]
    private float playerDeceleration = 4.0f;
    [SerializeField]
    private float inAirPlayerDecelerationMultiplier = 0.1f;

    // private float inAirMaximumSpeedMultiplier = 0.5f;

    [Header("Gravity variables")]
    [SerializeField]
    private float playerAscendingGravity = 1.053f;
    [SerializeField]
    private float playerDescendingGravity = 1.5f;

    [Header("Jump variables")]
    [SerializeField]
    private float playerJumpSpeed = 17.5f;
    [SerializeField]
    private bool isJumping;

    [SerializeField]
    private bool isGrounded;
    public bool GetIsGrounded {
        get => isGrounded;
    }
    [SerializeField]
    private float groundCheckDistance = 0.02f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private int defaultJumpBufferFrames = 10;
    [SerializeField]
    private int defaultCoyoteBufferFrames = 10;
    [SerializeField]
    private int jumpBufferFrames;
    [SerializeField]
    private int coyoteBufferFrames;

    private float epsilon = 1e-4f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<BoxCollider2D>();

        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        moveAction = inputActions.FindAction("Move");
        jumpAction = inputActions.FindAction("Jump");

        jumpAction.performed += JumpHandler;

        colliderSize = new Vector2(transform.localScale.x*cl.size.x, transform.localScale.y*cl.size.y);

        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Start() {
    }

    private void OnDestroy() {
        jumpAction.performed -= JumpHandler;
        inputActions.Disable();
    }

    private void Update() {
        inputDirection = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate() {     
        IsGrounded();   
        Move();
        Gravity();
        Jump();
    }

    private void Gravity() {
        if (rb.linearVelocityY > 0.0f) {
            rb.linearVelocityY -= playerAscendingGravity;
        } else if (rb.linearVelocityY <= 0.0f) {
            rb.linearVelocityY -= playerDescendingGravity;
        }

        if (rb.linearVelocityY > playerJumpSpeed) {
            rb.linearVelocityY = playerJumpSpeed;
        }
    }

    private void Jump() {
        if (isJumping) {
            if (isGrounded) {
                rb.linearVelocityY = playerJumpSpeed;
            } else {
                jumpBufferFrames = defaultJumpBufferFrames;
            }

            if (coyoteBufferFrames > 0) {
                rb.linearVelocityY = playerJumpSpeed;
                coyoteBufferFrames = 0;
            }

            isJumping = false;
        } else if (coyoteBufferFrames > 0) {
            coyoteBufferFrames--;
        }

        if (jumpBufferFrames > 0) {
            if (isGrounded) {
                rb.linearVelocityY = playerJumpSpeed;
                jumpBufferFrames = 0;
            } else {
                jumpBufferFrames--;
            }
        }
    }

    private void IsGrounded() {
        bool newFrameIsGrounded = Physics2D.BoxCast(transform.position, colliderSize, 0.0f, Vector2.down, groundCheckDistance, groundLayer).collider;

        if (!newFrameIsGrounded && isGrounded) {
            coyoteBufferFrames = defaultCoyoteBufferFrames;
        }

        isGrounded = newFrameIsGrounded;
    }

    private void Move() {
        rb.linearVelocityX = MathF.Round(rb.linearVelocityX, 2);

        if (Math.Abs(rb.linearVelocityX) < epsilon) {
            rb.linearVelocityX = 0.0f;
        }

        if (inputDirection.x != 0.0f) {
            if (inputDirection.x > 0 && rb.linearVelocityX < defaultPlayerSpeed || inputDirection.x < 0 && rb.linearVelocityX > -defaultPlayerSpeed) {
                Accelerate(Math.Sign(inputDirection.x));
            }
        } else if (inputDirection.x == 0.0f && Math.Abs(rb.linearVelocityX) > 0.0f) {
            Decelerate();
        }

        if (Math.Abs(rb.linearVelocityX) > defaultPlayerSpeed) {
            Decelerate();
        } 
    }

    private void Decelerate() {
        float finalDeceleration;
        if (isGrounded) {
            finalDeceleration = playerDeceleration*Math.Sign(rb.linearVelocityX);
        } else {
            finalDeceleration = playerDeceleration*inAirPlayerDecelerationMultiplier*Math.Sign(rb.linearVelocityX);
        }

        if (Math.Sign(rb.linearVelocityX)*Math.Sign(rb.linearVelocityX - finalDeceleration) < 0) {
            rb.linearVelocityX = 0.0f;
        } else {
            rb.linearVelocityX -= finalDeceleration;
        }
    }

    private void Accelerate(float direction) {
        float finalAcceleration;
        if (isGrounded) {
            finalAcceleration = playerAcceleration*direction;
        } else {
            finalAcceleration = playerAcceleration*inAirPlayerAccelerationMultiplier*direction;
        }
        rb.linearVelocityX += finalAcceleration;
    }

    private void JumpHandler(InputAction.CallbackContext context) {
        isJumping = true;
    }

    public void StopMovement() {
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmos() {
        int framesLookahead = 25;
        Vector2 pointFrom = transform.position;
        Vector2 pointTo;
        Vector2 velocity;
        if (!rb || isGrounded) {
            return;
        }
        velocity = rb.linearVelocity;

        for (int i = 0; i < framesLookahead; i++) {
            pointTo = pointFrom + velocity/50;
            Gizmos.DrawLine(pointFrom, pointTo);
            pointFrom = pointTo;

            if (velocity.y > 0.0f) {
                velocity.y -= playerAscendingGravity;
            } else {
                velocity.y -= playerDescendingGravity;
            }
        }
    }

}
