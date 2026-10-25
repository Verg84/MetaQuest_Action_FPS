using UnityEngine;

public class EnemyLaserAttack : MonoBehaviour
{
    [SerializeField] private EnemyBrain enemy;
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private Material laserMaterial;

    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float burstDuration = 0.15f;

    private LineRenderer beam;
    private float hideTime;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponent<EnemyBrain>();

        beam = gameObject.AddComponent<LineRenderer>();
        beam.useWorldSpace = true;
        beam.positionCount = 2;
        beam.startWidth = 0.025f;
        beam.endWidth = 0.025f;
        beam.numCapVertices = 4;
        beam.sharedMaterial = laserMaterial;
        beam.enabled = false;
    }

    private void OnEnable()
    {
        if (enemy != null)
            enemy.LaserShotRequested += Fire;
    }

    private void OnDisable()
    {
        if (enemy != null)
            enemy.LaserShotRequested -= Fire;

        if (beam != null)
            beam.enabled = false;
    }

    private void Update()
    {
        if (beam.enabled && Time.time >= hideTime)
            beam.enabled = false;
    }

    private void Fire(Vector3 aimPosition)
    {
        if (laserOrigin == null)
            return;

        Vector3 origin = laserOrigin.position;
        Vector3 direction = aimPosition - origin;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        Vector3 end = origin + direction * maxDistance;

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            maxDistance,
            hitLayers,
            QueryTriggerInteraction.Ignore))
        {
            end = hit.point;

            PlayerHealth playerHealth =
                hit.collider.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }

        beam.SetPosition(0, origin);
        beam.SetPosition(1, end);
        beam.enabled = true;

        hideTime = Time.time + burstDuration;
    }
}