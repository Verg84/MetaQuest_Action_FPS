using TMPro;
using UnityEngine;

public class WristHealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        if (playerHealth == null || healthText == null)
            return;

        float health = playerHealth.CurrentHealth;

        healthText.text = playerHealth.IsDead
            ? "DEAD"
            : $"HP {Mathf.CeilToInt(health)}";

        healthText.color = health <= 25f
            ? Color.red
            : health <= 50f
                ? Color.yellow
                : Color.green;
    }
}