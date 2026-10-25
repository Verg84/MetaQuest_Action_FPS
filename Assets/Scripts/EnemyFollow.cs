using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float attackRange = 1.7f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackInterval = 1f;

    private float nextAttackTime;

    private void Update()
    {
        if (player == null || playerHealth == null)
            return;

        if (playerHealth.IsDead)
            return;

        Vector3 direction = player.position - transform.position;

        // Ignore headset height when measuring horizontal distance.
        direction.y = 0f;
        float distance = direction.magnitude;

        if (distance > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction);

        if (distance > stoppingDistance)
        {
            float step = Mathf.Min(
                moveSpeed * Time.deltaTime,
                distance - stoppingDistance
            );

            transform.position += direction.normalized * step;
        }

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            nextAttackTime =
                Time.time + Mathf.Max(0.1f, attackInterval);

            playerHealth.TakeDamage(attackDamage);
        }
    }
}