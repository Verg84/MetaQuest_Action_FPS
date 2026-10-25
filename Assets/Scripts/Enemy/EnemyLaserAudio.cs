using UnityEngine;

public class EnemyLaserAudio : MonoBehaviour
{
    [SerializeField] private EnemyBrain enemy;
    [SerializeField] private AudioSource chargeAudio;
    [SerializeField] private AudioSource fireAudio;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponentInParent<EnemyBrain>();
    }

    private void OnEnable()
    {
        if (enemy == null)
            return;

        enemy.ChargeStarted += PlayCharge;
        enemy.ChargeStopped += StopCharge;
        enemy.LaserShotRequested += PlayFire;
    }

    private void OnDisable()
    {
        if (enemy != null)
        {
            enemy.ChargeStarted -= PlayCharge;
            enemy.ChargeStopped -= StopCharge;
            enemy.LaserShotRequested -= PlayFire;
        }

        StopCharge();

        if (fireAudio != null)
            fireAudio.Stop();
    }

    private void PlayCharge(float duration)
    {
        if (chargeAudio == null || chargeAudio.clip == null)
            return;

        chargeAudio.Stop();

        // Match the clip to the configured charging duration.
        chargeAudio.pitch =
            chargeAudio.clip.length / Mathf.Max(0.01f, duration);

        chargeAudio.Play();
    }

    private void StopCharge()
    {
        if (chargeAudio != null)
            chargeAudio.Stop();
    }

    private void PlayFire(Vector3 aimPosition)
    {
        StopCharge();

        if (fireAudio != null && fireAudio.clip != null)
            fireAudio.PlayOneShot(fireAudio.clip);
    }
}