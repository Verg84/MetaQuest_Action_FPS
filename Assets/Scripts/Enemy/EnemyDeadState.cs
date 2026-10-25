public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyBrain enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.StopChargeEffect();
        enemy.PrepareForDeath();
        enemy.PlayAnimation("Base Layer.Dead");
    }
}