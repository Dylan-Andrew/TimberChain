using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float initialSpawnHeight = 0f;
    [SerializeField]
    private float minX = -5f;
    [SerializeField]
    private float maxX = 5f;
    [SerializeField]
    private float spawnBufferDistance = 10f;
    [SerializeField]
    private float destructionDistance = 40f;

    private float nextSpawnHeight;
    private int difficultyLevel = 1;
    [SerializeField]
    private float difficultyIncreaseThreshold = 25f;
    [SerializeField]
    private PlayerController playerController;

    private List<GameObject> spawnedBranches = new List<GameObject>();

    private void Start()
    {
        nextSpawnHeight = initialSpawnHeight;

        for (int i = 0; i < 5; i++)
        {
            SpawnPrefab();
        }
    }

    private void Update()
    {
        float spawnHeightThreshold = playerTransform.position.y + (Camera.main.orthographicSize * 2) + spawnBufferDistance;

        while (nextSpawnHeight <= spawnHeightThreshold)
        {
            SpawnPrefab();
        }

        if (playerTransform.position.y >= difficultyLevel * difficultyIncreaseThreshold)
        {
            IncreaseDifficulty();
        }

        DestroyFarBranches();
    }

    private void SpawnPrefab()
    {
        float previousX = float.MinValue;

        for (int i = 0; i < 2; i++)
        {
            GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

            float randomX;
            do
            {
                randomX = Random.Range(minX, maxX);
            } while (Mathf.Abs(randomX - previousX) < 5f);

            Vector3 spawnPosition = new Vector3(randomX, nextSpawnHeight, 0);
            GameObject spawnedBranch = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnedBranches.Add(spawnedBranch);

            previousX = randomX;
        }

        nextSpawnHeight += Random.Range(3f, 5f);
    }

    private void DestroyFarBranches()
    {
        float minHeightThreshold = playerTransform.position.y - destructionDistance;

        for (int i = spawnedBranches.Count - 1; i >= 0; i--)
        {
            if (spawnedBranches[i] != null && spawnedBranches[i].transform.position.y < minHeightThreshold)
            {
                Destroy(spawnedBranches[i]);
                spawnedBranches.RemoveAt(i);
            }
        }
    }

    private void IncreaseDifficulty()
    {
        difficultyLevel++;
        float newDestructionDelay = Mathf.Lerp(5f, 0.75f, (float)difficultyLevel / 10);
        playerController.UpdatePlayerSpeed();
        BreakableBranch.UpdateDestructionDelay(newDestructionDelay);
    }
}
