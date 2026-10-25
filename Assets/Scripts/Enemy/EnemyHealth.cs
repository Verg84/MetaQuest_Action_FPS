using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private EnemyBrain enemy;
    private float currentHealth;
    private bool dead;

    private void Awake()
    {
        enemy = GetComponent<EnemyBrain>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (dead || damage <= 0f)
            return;

        currentHealth = Mathf.Max(
            0f,
            currentHealth - damage
        );

        if (currentHealth <= 0f)
        {
            dead = true;
            enemy?.Die();
            return;
        }

        enemy?.Stun();
    }
}