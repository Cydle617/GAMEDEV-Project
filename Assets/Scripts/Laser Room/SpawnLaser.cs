using UnityEngine;

public class SpawnLaser : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] laserPrefabs; // Array of different laser shapes/patterns
    [SerializeField] private float spawnInterval = 2.5f; // Seconds between spawns
    [SerializeField] private Transform spawnPoint;

    private float spawnTimer = 0f;
    private bool isSpawning = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnRandomLaser();
            spawnTimer = 0f;
        }
    }

    private void SpawnRandomLaser()
    {
        if (laserPrefabs == null || laserPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, laserPrefabs.Length);

        // Explicitly use spawnPoint's fixed position and rotation
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion spawnRot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        Instantiate(laserPrefabs[randomIndex], spawnPos, spawnRot);
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}
