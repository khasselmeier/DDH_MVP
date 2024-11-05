using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [Header("Gem Prefabs")]
    public GameObject baseGemPrefab;
    public GameObject highGemPrefab;

    [Header("Spawn Settings")]
    public int numberOfBaseGems = 6; // Number of base gems to spawn
    public int numberOfHighGems = 2; // Number of high gems to spawn

    [Header("Spawn Locations")]
    public List<Transform> spawnLocations = new List<Transform>(); // List of spawn locations

    private void Start()
    {
        SpawnGems();
    }

    private void SpawnGems()
    {
        if (spawnLocations.Count == 0)
        {
            Debug.LogWarning("No spawn locations assigned.");
            return;
        }

        List<Transform> availableLocations = new List<Transform>(spawnLocations);

        // Spawn base gems
        SpawnGemType(baseGemPrefab, numberOfBaseGems, availableLocations);

        // Spawn high gems
        SpawnGemType(highGemPrefab, numberOfHighGems, availableLocations);
    }

    private void SpawnGemType(GameObject gemPrefab, int count, List<Transform> availableLocations)
    {
        for (int i = 0; i < count; i++)
        {
            if (availableLocations.Count == 0)
            {
                Debug.LogWarning("Not enough spawn locations for all gems.");
                return;
            }

            // Select a random location from available locations
            int randomIndex = Random.Range(0, availableLocations.Count);
            Transform spawnLocation = availableLocations[randomIndex];

            // Spawn gem at the selected location
            Instantiate(gemPrefab, spawnLocation.position, Quaternion.identity);

            // Remove used location from the list
            availableLocations.RemoveAt(randomIndex);
        }
    }
}