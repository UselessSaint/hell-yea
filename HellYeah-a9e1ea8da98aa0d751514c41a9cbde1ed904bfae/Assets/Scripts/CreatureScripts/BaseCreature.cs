using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseCreature : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected List<Transform> patrolPoints;
    protected int currentPatrolPoint = 0;
    [SerializeField]
    protected float movementSpeed = 5.0f;

    protected Rigidbody2D rb;

    public abstract void Damage(float damageValue);

    protected void Patrol() {
        Vector2 targetPosition = patrolPoints[currentPatrolPoint].position;
        Vector2 distance = targetPosition - (Vector2) transform.position;
        distance.y = 0;

        if (distance.magnitude > movementSpeed/25) {
            Vector2 direction = distance.normalized;
            rb.linearVelocity = direction*movementSpeed;
        } else {
            if (currentPatrolPoint < patrolPoints.Count-1) {
                currentPatrolPoint++;
            } else {
                currentPatrolPoint = 0;
            }
        }
    }
}
