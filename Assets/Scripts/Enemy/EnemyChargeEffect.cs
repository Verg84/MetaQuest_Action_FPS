using UnityEngine;

public class EnemyChargeEffect : MonoBehaviour
{
    [SerializeField] private EnemyBrain enemy;
    [SerializeField] private Transform orb;

    [SerializeField] private float startSize = 0.08f;
    [SerializeField] private float endSize = 0.4f;

    private bool charging;
    private float elapsed;
    private float chargeDuration;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponentInParent<EnemyBrain>();
    }

    private void OnEnable()
    {
        Hide();

        if (enemy == null)
            return;

        enemy.ChargeStarted += StartCharging;
        enemy.ChargeStopped += Hide;
    }

    private void OnDisable()
    {
        if (enemy != null)
        {
            enemy.ChargeStarted -= StartCharging;
            enemy.ChargeStopped -= Hide;
        }

        Hide();
    }

    private void StartCharging(float duration)
    {
        if (orb == null)
            return;

        charging = true;
        elapsed = 0f;
        chargeDuration = Mathf.Max(0.01f, duration);

        orb.localScale = Vector3.one * startSize;
        orb.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!charging || orb == null)
            return;

        elapsed += Time.deltaTime;

        float progress = Mathf.Clamp01(elapsed / chargeDuration);
        float size = Mathf.Lerp(startSize, endSize, progress);

        orb.localScale = Vector3.one * size;
    }

    private void Hide()
    {
        charging = false;

        if (orb != null)
            orb.gameObject.SetActive(false);
    }
}