using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private ParticleSystem impactPrefab;
    [SerializeField] private LayerMask hitLayers = Physics.DefaultRaycastLayers;

    private float damage;

    public void Initialize(float shotDamage)
    {
        damage = shotDamage;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        float distance = speed * Time.deltaTime;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit hit,
            distance,
            hitLayers,
            QueryTriggerInteraction.Ignore))
        {
            if (impactPrefab != null)
            {
                Instantiate(
                    impactPrefab,
                    hit.point + hit.normal * 0.01f,
                    Quaternion.LookRotation(hit.normal)
                );
            }

            EnemyHitFragments fragments = hit.collider.GetComponentInParent<EnemyHitFragments>();

            if (fragments != null)
            {
                fragments.SpawnFragments(
                    hit.point,
                    hit.normal
                );
            }

            ShootableTarget target = hit.collider.GetComponentInParent<ShootableTarget>();

            if (target != null)
                target.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        transform.position += transform.forward * distance;
    }
}