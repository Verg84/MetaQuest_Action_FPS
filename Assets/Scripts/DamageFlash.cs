using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Image overlay;
    [SerializeField, Range(0f, 1f)] private float opacity = 0.2f;
    [SerializeField] private float duration = 0.15f;

    private float remainingTime;

    private void Awake()
    {
        SetAlpha(0f);
    }

    public void Flash()
    {
        remainingTime = Mathf.Max(0.01f, duration);
        SetAlpha(opacity);
    }

    private void Update()
    {
        if (remainingTime <= 0f)
            return;

        remainingTime = Mathf.Max(
            0f,
            remainingTime - Time.unscaledDeltaTime
        );

        float progress =
            remainingTime / Mathf.Max(0.01f, duration);

        SetAlpha(opacity * progress);
    }

    private void OnDisable()
    {
        remainingTime = 0f;
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (overlay != null)
            overlay.color = new Color(1f, 0f, 0f, alpha);
    }
}