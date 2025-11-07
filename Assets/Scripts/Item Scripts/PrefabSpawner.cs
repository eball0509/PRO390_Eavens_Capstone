using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab;
    public int minSpawn = 100;
    public int maxSpawn = 1000;
    public bool alignToSlope = false;
}

public class PrefabSpawner : MonoBehaviour
{
    public SpawnablePrefab[] spawnablePrefabs;
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);

    public float raycastHeight = 10f;
    public LayerMask groundMask;

    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        if (spawnablePrefabs == null || spawnablePrefabs.Length == 0) return;

        foreach (var item in spawnablePrefabs)
        {
            if (item.prefab == null) continue;

            int count = Random.Range(item.minSpawn, item.maxSpawn + 1);

            for (int i = 0; i < count; i++)
            {
                // Random X/Z inside box
                float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
                float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);
                Vector3 randomPosition = transform.position + new Vector3(randomX, 0f, randomZ);

                // Raycast down from above
                Vector3 rayOrigin = randomPosition + Vector3.up * raycastHeight;
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundMask))
                {
                    Vector3 spawnPoint = hit.point;

                    Quaternion rotation;
                    if (item.alignToSlope)
                    {
                        rotation = Quaternion.FromToRotation(Vector3.up, hit.normal)
                                   * Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    }
                    else
                    {
                        rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    }

                    GameObject obj = Instantiate(item.prefab, spawnPoint, rotation);
                    obj.transform.parent = transform;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 drawPos = transform.position;
        drawPos.y += 0.1f;
        Gizmos.DrawWireCube(drawPos, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.z));
    }
}
