using System;
using UnityEngine;

[Serializable]
public class LilBallProjectile : MonoBehaviour
{
    [SerializeField]
    private float projectileDamage = 75.0f;
    private float projectileSpeed = 25.0f;
    private float projectileTTL = 3.0f;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, projectileTTL);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            return;
        }

        IDamageable creature = collision.GetComponent<IDamageable>();
        creature?.Damage(projectileDamage);
        Destroy(gameObject);
    }

    public void Init(Vector2 target) {
        Vector2 direction = (target - (Vector2) transform.position).normalized;
        rb.linearVelocity = direction*projectileSpeed;
    }
}
