public abstract class EnemyState
{
    protected readonly EnemyBrain enemy;

    protected EnemyState(EnemyBrain enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter() { }

    public virtual void Tick() { }

    public virtual void Exit() { }
}