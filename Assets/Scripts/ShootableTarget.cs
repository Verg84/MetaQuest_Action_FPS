using UnityEngine;

public class ShootableTarget : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
            return;

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            return;
        }

        health -= damage;

        if (health <= 0f)
            Destroy(gameObject);
    }
}