public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBrain enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.PlayAnimation("Base Layer.Walk");
    }

    public override void Tick()
    {
        if (!enemy.HasPlayer ||
            enemy.DistanceToPlayer > enemy.DetectionRange)
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
            return;
        }

        if (enemy.DistanceToPlayer <= enemy.ShootingDistance)
        {
            enemy.ChangeState(new EnemyAttackState(enemy));
            return;
        }

        enemy.MoveTowardsPlayer();
    }
}