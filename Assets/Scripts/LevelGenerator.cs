using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs; // Array to hold the different prefabs
    [SerializeField]
    private Transform playerTransform; // Reference to the player’s transform
    [SerializeField]
    private float initialSpawnHeight = 0f; // Initial height to start spawning
    [SerializeField]
    private float minX = -5f; // Minimum X position for spawning
    [SerializeField]
    private float maxX = 5f; // Maximum X position for spawning
    [SerializeField]
    private float spawnBufferDistance = 10f; // Distance above the visible area to spawn prefabs

    private float nextSpawnHeight;
    private int difficultyLevel = 1; // Tracks the difficulty level
    [SerializeField]
    private float difficultyIncreaseThreshold = 50f; // Distance interval to increase difficulty

    private void Start()
    {
        nextSpawnHeight = initialSpawnHeight;

        // Spawn a few initial prefabs to ensure enough content
        for (int i = 0; i < 5; i++)
        {
            SpawnPrefab();
        }
    }

    private void Update()
    {
        // Calculate the spawn position based on player position and camera size
        float spawnHeightThreshold = playerTransform.position.y + (Camera.main.orthographicSize * 2) + spawnBufferDistance;

        // Spawn a new prefab if the next spawn height is within or below the threshold
        while (nextSpawnHeight <= spawnHeightThreshold)
        {
            SpawnPrefab();
        }

        // Increase difficulty as the player progresses upward
        if (playerTransform.position.y >= difficultyLevel * difficultyIncreaseThreshold)
        {
            IncreaseDifficulty();
        }
    }

    private void SpawnPrefab()
    {
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        float randomX = Random.Range(minX, maxX);

        Vector3 spawnPosition = new Vector3(randomX, nextSpawnHeight, 0);
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        nextSpawnHeight += Random.Range(3f, 5f);
    }

    private void IncreaseDifficulty()
    {
        difficultyLevel++;

        float newDestructionDelay = Mathf.Lerp(5f, 0.75f, (float)difficultyLevel / 10); // Adjust the divisor as needed for smoother or faster transitions
        BreakableBranch.UpdateDestructionDelay(newDestructionDelay);
    }
}
