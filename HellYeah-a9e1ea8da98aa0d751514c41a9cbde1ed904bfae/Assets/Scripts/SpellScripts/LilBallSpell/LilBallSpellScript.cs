using System;
using UnityEngine;

[Serializable]
public class LilBallSpellScript : BaseSpellScript
{
    [SerializeField]
    private GameObject lilBall;
    [SerializeField]
    private float attacksPerSecond = 6.0f;
    [SerializeField]
    private float spellCooldown;
    [SerializeField]
    private float spellCooldownTimer = 0.0f;

    void Awake() {
        lilBall = Resources.Load("Prefabs/lilBall") as GameObject;
        spellCooldown = 1/attacksPerSecond;
    }

    public override void DoWhileIsInProgress() {
        if (spellCooldownTimer <= 0.0f) {
            ThrowLilBall();
            spellCooldownTimer = spellCooldown;
        }
    }

    private void FixedUpdate() {
        if (spellCooldownTimer > 0.0f) {
            spellCooldownTimer -= Time.fixedDeltaTime;
            if (spellCooldownTimer < 0.0f) {
                spellCooldownTimer = 0.0f;
            }
        }
    }

    private void ThrowLilBall() {
        playerPos += Vector2.up/2;
        GameObject ball = Instantiate(lilBall, playerPos, Quaternion.identity);
        ball.GetComponent<LilBallProjectile>().Init(mousePos);
    }

}
