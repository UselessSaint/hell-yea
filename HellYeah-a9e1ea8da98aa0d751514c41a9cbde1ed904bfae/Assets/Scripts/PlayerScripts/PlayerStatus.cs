using System;
using UnityEngine;

[Serializable]
public class PlayerStatus : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maximumHealth = 100.0f;
    [SerializeField]
    private float currentHealth;

    void Awake() {
        currentHealth = maximumHealth;
    }

    void FixedUpdate() {
        if (currentHealth < 0.0f) {
            Destroy(gameObject);
        }
    }

    public void Damage(float damageValue)
    {
        currentHealth -= damageValue;
    }
}
