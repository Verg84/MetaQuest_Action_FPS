public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBrain enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.PlayAnimation("Base Layer.Idle");
    }

    public override void Tick()
    {
        if (!enemy.HasPlayer)
            return;

        float distance = enemy.DistanceToPlayer;

        if (distance > enemy.DetectionRange)
            return;

        enemy.FacePlayer();

        if (distance <= enemy.ShootingDistance)
        {
            enemy.ChangeState(new EnemyAttackState(enemy));
        }
        else if (distance > enemy.ResumeChaseDistance)
        {
            enemy.ChangeState(new EnemyChaseState(enemy));
        }
    }
}