using UnityEngine;
using UnityEngine.InputSystem;

public class VRGun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private LaserBolt boltPrefab;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip firingSound;

    private InputAction trigger;
    private float nextFireTime;

    private void Awake()
    {
        trigger = new InputAction(
            name: "Fire",
            type: InputActionType.Value,
            binding: "<XRController>{RightHand}/trigger"
        );
    }

    private void OnEnable()
    {
        trigger.Enable();
    }

    private void OnDisable()
    {
        trigger.Disable();
    }

    private void Update()
    {
        if (muzzle == null || boltPrefab == null)
            return;

        if (trigger.ReadValue<float>() > 0.5f
            && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireInterval;

            LaserBolt bolt = Instantiate(
                boltPrefab,
                muzzle.position,
                muzzle.rotation
            );

            bolt.Initialize(damage);

            if (audioSource != null && firingSound != null)
            {
                audioSource.PlayOneShot(firingSound);
            }
        }
    }

    private void OnDestroy()
    {
        trigger?.Dispose();
    }
}