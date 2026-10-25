using UnityEngine;

public class EnemyStunnedState : EnemyState
{
    private readonly float duration;
    private float endTime;

    public EnemyStunnedState(
        EnemyBrain enemy,
        float duration
    ) : base(enemy)
    {
        this.duration = duration;
    }

    public override void Enter()
    {
        enemy.StopChargeEffect();
        enemy.PlayAnimation("Base Layer.Stunned");

        endTime = Time.time + duration;

        Debug.Log("Enemy stunned");
    }

    public override void Tick()
    {
        if (Time.time >= endTime)
            enemy.ChangeState(new EnemyIdleState(enemy));
    }
}