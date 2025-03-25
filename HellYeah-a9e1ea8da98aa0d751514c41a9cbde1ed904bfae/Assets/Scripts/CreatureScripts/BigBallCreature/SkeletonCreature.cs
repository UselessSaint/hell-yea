using System;
using UnityEngine;

[Serializable]
public class SkeletonCreature : BaseCreature
{
    [SerializeField]
    private float maximumHealth = 100;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float gravity = 1.5f;
    private SpriteRenderer sr;

    void Awake() {
        currentHealth = maximumHealth;
        rb = GetComponent<Rigidbody2D>();
        // playerLayer = LayerMask.GetMask("Player");
        sr = GetComponent<SpriteRenderer>();
    }

    public override void Damage(float damageValue) {
        currentHealth -= damageValue;
    }

    void FixedUpdate() {
        if (currentHealth < 0.0f) {
            Destroy(gameObject);
        }

        Patrol();
        Gravity();

        if (rb.linearVelocityX < 0.0f) {
            sr.flipX = true;
        } else {
            sr.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Vector2 pushDirection = collision.gameObject.transform.position - transform.position;
            pushDirection.y = 0.5f;
            pushDirection.x = 0.5f*Math.Sign(pushDirection.x);
            pushDirection = pushDirection.normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity += pushDirection*20.0f;
        }
    }

    private void Gravity() {
        rb.linearVelocityY = -gravity;
    }
}
