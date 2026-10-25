using System.Collections.Generic;
using UnityEngine;

public class EnemyHitFragments : MonoBehaviour
{
    [SerializeField] private HitFragment[] fragmentPrefabs;

    [SerializeField] private int minimumCount = 2;
    [SerializeField] private int maximumCount = 4;

    [SerializeField] private float spawnOffset = 0.06f;
    [SerializeField] private float spread = 0.6f;

    // Shared limit across all enemies.
    private const int MaxActiveFragments = 40;

    private static readonly List<HitFragment> activeFragments =
        new List<HitFragment>();

    public void SpawnFragments(
        Vector3 hitPosition,
        Vector3 surfaceNormal)
    {
        if (fragmentPrefabs == null ||
            fragmentPrefabs.Length == 0)
        {
            return;
        }

        // Remove references to fragments that have expired.
        activeFragments.RemoveAll(fragment => fragment == null);

        int typeIndex = Random.Range(0, fragmentPrefabs.Length);
        HitFragment selectedPrefab = fragmentPrefabs[typeIndex];

        if (selectedPrefab == null)
            return;

        int minimum = Mathf.Max(1, minimumCount);
        int maximum = Mathf.Max(minimum, maximumCount);
        int count = Random.Range(minimum, maximum + 1);

        Vector3 normal = surfaceNormal.sqrMagnitude > 0.001f
            ? surfaceNormal.normalized
            : Vector3.up;

        Vector3 spawnPosition =
            hitPosition + normal * spawnOffset;

        for (int i = 0; i < count; i++)
        {
            if (activeFragments.Count >= MaxActiveFragments)
                break;

            HitFragment fragment = Instantiate(
                selectedPrefab,
                spawnPosition,
                Random.rotation
            );

            Vector3 direction =
                normal
                + Random.insideUnitSphere * spread
                + Vector3.up * 0.25f;

            // Keep the burst directed out of the hit surface.
            if (Vector3.Dot(direction, normal) < 0.1f)
                direction = normal + Vector3.up * 0.25f;

            fragment.Launch(direction);
            activeFragments.Add(fragment);
        }
    }
}