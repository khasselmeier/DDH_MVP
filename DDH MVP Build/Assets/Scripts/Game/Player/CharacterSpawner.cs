using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject characterOnePrefab;  // Prefab for Moldrock
    public GameObject characterTwoPrefab;  // Prefab for Thalgrim
    public Transform spawnPoint;

    public Image moldrockUIImage;
    public Image thalgrimUIImage;

    void Start()
    {
        // Get the selected character from PlayerPrefs
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Moldrock");
        Debug.Log("Selected Character from PlayerPrefs: " + selectedCharacter);

        // Spawn the character based on the selection
        SpawnCharacter(selectedCharacter);

        // Update UI based on the selected character
        UpdateCharacterUI(selectedCharacter);
    }

    public void SpawnCharacter(string selectedCharacter)
    {
        GameObject selectedPrefab = null;

        // Determine which character to spawn based on the selected character type
        if (selectedCharacter == "Moldrock")
        {
            selectedPrefab = characterOnePrefab;
            Debug.Log("Moldrock prefab selected");
        }
        else if (selectedCharacter == "Thalgrim")
        {
            selectedPrefab = characterTwoPrefab;
            Debug.Log("Thalgrim prefab selected");
        }

        // Spawn the character at the spawn point
        if (selectedPrefab != null && spawnPoint != null)
        {
            GameObject playerInstance = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
            playerInstance.tag = "Player"; // makes sure the player is tagged as "Player"
            //Debug.Log($"Spawned {selectedCharacter} and tagged as: {playerInstance.tag}");
        }
        else
        {
            if (selectedPrefab == null)
            {
                Debug.LogError("Selected prefab is null");
            }

            if (spawnPoint == null)
            {
                Debug.LogError("Spawn point is missing");
            }
        }
    }

    private void UpdateCharacterUI(string selectedCharacter)
    {
        if (selectedCharacter == "Moldrock")
        {
            moldrockUIImage.gameObject.SetActive(true);
            thalgrimUIImage.gameObject.SetActive(false);
            //Debug.Log("Moldrock UI image displayed");
        }
        else if (selectedCharacter == "Thalgrim")
        {
            thalgrimUIImage.gameObject.SetActive(true);
            moldrockUIImage.gameObject.SetActive(false);
            //Debug.Log("Thalgrim UI image displayed");
        }
        else
        {
            //Debug.LogError("Unknown character selected, no UI image to display.");
        }
    }
}