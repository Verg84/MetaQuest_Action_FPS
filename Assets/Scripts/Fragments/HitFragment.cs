using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HitFragment : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 2f;
    [SerializeField] private float spinSpeed = 12f;
    [SerializeField] private float lifetime = 1.5f;

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, Mathf.Max(0.1f, lifetime));
    }

    public void Launch(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            direction = Vector3.up;

        body.linearVelocity =
            direction.normalized * launchSpeed;

        body.angularVelocity =
            Random.onUnitSphere * spinSpeed;
    }
}