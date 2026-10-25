using UnityEngine;

public class SpaceDebrisMotion : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationSpeed =
        new Vector3(2f, 4f, 1f);

    [SerializeField]
    private Vector3 driftDirection =
        new Vector3(1f, 0.3f, 0f);

    [SerializeField] private float driftDistance = 2f;
    [SerializeField] private float driftSpeed = 0.15f;

    private Vector3 startPosition;
    private float phase;

    private void Start()
    {
        startPosition = transform.localPosition;
        phase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        transform.Rotate(
            rotationSpeed * Time.deltaTime,
            Space.Self
        );

        float offset =
            Mathf.Sin(Time.time * driftSpeed + phase)
            * driftDistance;

        transform.localPosition =
            startPosition + driftDirection.normalized * offset;
    }
}