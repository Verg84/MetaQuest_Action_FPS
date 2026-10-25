using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private enum AttackPhase
    {
        Charging,
        Cooldown
    }

    private AttackPhase phase;
    private float phaseEndTime;

    public EnemyAttackState(EnemyBrain enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        // No separate attack animation is needed for a floating emitter.
        enemy.PlayAnimation("Base Layer.Idle");
        BeginCharge();
    }

    public override void Tick()
    {
        if (!enemy.HasPlayer ||
            enemy.DistanceToPlayer > enemy.DetectionRange)
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
            return;
        }

        enemy.FacePlayer();

        if (phase == AttackPhase.Charging)
        {
            // Cancel the charge if the player leaves firing range.
            if (enemy.DistanceToPlayer > enemy.ShootingDistance)
            {
                enemy.ChangeState(new EnemyIdleState(enemy));
                return;
            }

            if (Time.time >= phaseEndTime)
            {
                // Capture the aim point once, rather than tracking
                // the player throughout the future beam burst.
                Vector3 aimPosition = enemy.PlayerAimPosition;

                enemy.StopChargeEffect();
                enemy.RequestLaserShot(aimPosition);

                phase = AttackPhase.Cooldown;
                phaseEndTime = Time.time + enemy.AttackCooldown;
            }
        }
        else if (Time.time >= phaseEndTime)
        {
            if (enemy.DistanceToPlayer <= enemy.ShootingDistance)
            {
                BeginCharge();
            }
            else if (
                enemy.DistanceToPlayer > enemy.ResumeChaseDistance)
            {
                enemy.ChangeState(new EnemyChaseState(enemy));
            }
            else
            {
                enemy.ChangeState(new EnemyIdleState(enemy));
            }
        }
    }

    private void BeginCharge()
    {
        phase = AttackPhase.Charging;
        phaseEndTime = Time.time + enemy.ChargeDuration;

        enemy.BeginChargeEffect();

        Debug.Log("Enemy charging laser");
    }

    public override void Exit()
    {
        enemy.StopChargeEffect();
    }
}