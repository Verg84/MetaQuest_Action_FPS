using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;

    [SerializeField] private CharacterController playerBody;

    [Header("Movement")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float shootingDistance = 8f;
    [SerializeField] private float resumeChaseDistance = 9f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float turnSpeed = 180f;

    [Header("Stun")]
    [SerializeField] private float stunDuration = 0.6f;

    [Header("Death")]
    [SerializeField] private float deathAnimationDuration = 3f;
    [SerializeField] private float corpseStayDuration = 2f;

    public bool IsDead { get; private set; }

    private EnemyState currentState;

    public bool HasPlayer => player != null;
    public float DetectionRange => detectionRange;
    public float ShootingDistance => shootingDistance;
    public float ResumeChaseDistance => resumeChaseDistance;

    [Header("Laser Attack")]
    [SerializeField] private float chargeDuration = 0.8f;
    [SerializeField] private float attackCooldown = 1.5f;

    public float ChargeDuration => Mathf.Max(0.01f, chargeDuration);
    public float AttackCooldown => Mathf.Max(0.01f, attackCooldown);

    // The future laser component will listen to this event.
    public event System.Action<Vector3> LaserShotRequested;

    public Vector3 PlayerAimPosition
    {
        get
        {
            if (playerBody != null && playerBody.enabled)
                return playerBody.bounds.center;

            return player != null
                ? player.position - Vector3.up * 0.5f
                : transform.position;
        }
    }

    public void RequestLaserShot(Vector3 aimPosition)
    {
        Debug.Log("Enemy laser shot requested", this);
        LaserShotRequested?.Invoke(aimPosition);
    }

    private Vector3 DirectionToPlayer
    {
        get
        {
            if (!HasPlayer)
                return Vector3.zero;

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            return direction;
        }
    }

    public float DistanceToPlayer => DirectionToPlayer.magnitude;

    public event System.Action<float> ChargeStarted;
    public event System.Action ChargeStopped;

    public void BeginChargeEffect()
    {
        ChargeStarted?.Invoke(ChargeDuration);
    }

    public void StopChargeEffect()
    {
        ChargeStopped?.Invoke();
    }

    private void Start()
    {
        if (animator == null || player == null)
        {
            Debug.LogError(
                "EnemyBrain: assign Animator and Player.",
                this
            );

            enabled = false;
            return;
        }

        ChangeState(new EnemyIdleState(this));
    }

    private void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(EnemyState nextState)
    {
        if (nextState == null)
            return;

        if (IsDead && !(nextState is EnemyDeadState))
            return;

        currentState?.Exit();
        currentState = nextState;
        currentState.Enter();
    }

    public void PlayAnimation(string stateName)
    {
        animator.CrossFadeInFixedTime(stateName, 0.1f, 0);
    }

    public void FacePlayer()
    {
        Vector3 direction = DirectionToPlayer;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    public void MoveTowardsPlayer()
    {
        FacePlayer();

        float step = Mathf.Min(
            moveSpeed * Time.deltaTime,
            Mathf.Max(0f, DistanceToPlayer - shootingDistance)
        );

        transform.position += DirectionToPlayer.normalized * step;
    }

    public void Stun()
    {
        if (IsDead)
            return;

        if (!isActiveAndEnabled ||
            currentState == null ||
            currentState is EnemyStunnedState)
        {
            return;
        }

        ChangeState(
            new EnemyStunnedState(
                this,
                Mathf.Max(0.01f, stunDuration)
            )
        );
    }

    public void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        ChangeState(new EnemyDeadState(this));
    }

    public void PrepareForDeath()
    {
        foreach (Collider enemyCollider
                 in GetComponentsInChildren<Collider>())
        {
            enemyCollider.enabled = false;
        }

        float removalDelay =
            Mathf.Max(0.1f, deathAnimationDuration)
            + Mathf.Max(0f, corpseStayDuration);

        Destroy(gameObject, removalDelay);
    }
}