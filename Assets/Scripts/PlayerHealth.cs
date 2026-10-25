using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private DamageFlash damageFlash;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);

        if (damageFlash != null)
            damageFlash.Flash();

        Debug.Log($"Player health: {CurrentHealth}");

        if (IsDead)
            Debug.Log("GAME OVER");
    }
}