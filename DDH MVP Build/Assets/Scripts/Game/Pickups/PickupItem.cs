using UnityEngine;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    public enum ItemType
    {
        Rock,
        Gold
    }

    public ItemType itemType;
    public int amount = 1; // Amount to add to the player's inventory

    private bool isPlayerInRange = false;
    private PlayerBehavior player; // Reference to the player

    [Header("UI Elements")]
    public GameObject pickupPanel; // UI panel to show interact button when in pickup range

    [Header("Outline Effect")]
    public Material outlineMaterial; // Material for the outline effect
    public Material originalMaterial; // Original material of the object
    private Renderer objectRenderer; // Renderer of the item

    private void Start()
    {
        if (itemType == ItemType.Rock)
        {
            amount = Random.Range(1, 10); // Random value for rocks
        }
        else if (itemType == ItemType.Gold)
        {
            amount = Random.Range(3, 12); // Random value for gold
        }

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(false); // Hide the UI panel initially
        }

        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material; // Store the original material
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.GetComponent<PlayerBehavior>();

            // Show the panel when the player is in range
            if (pickupPanel != null)
            {
                pickupPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null; // Clears player reference

            // Hide the pickup panel when the player leaves the range
            if (pickupPanel != null)
            {
                pickupPanel.SetActive(false);
            }

            DisableOutline(); // Remove outline when out of range
        }
    }

    private void Update()
    {
        // Handle item pickup
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Rock:
                        player.AddAmmo(amount); // Add ammo in PlayerBehavior
                        break;
                    case ItemType.Gold:
                        player.AddGold(amount); // Add gold in PlayerBehavior
                        break;
                }

                // Update the UI
                GameUI.instance.UpdateAmmoText(); // Update ammo UI
                GameUI.instance.UpdateGoldText(player.gold); // Update gold UI

                if (pickupPanel != null)
                {
                    pickupPanel.SetActive(false);
                }

                Destroy(gameObject); // Destroy item after collection
            }
        }

        // Raycast for crosshair detection
        CheckForHover();
    }

    private void CheckForHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                EnableOutline(); // Enable outline when hovered
            }
            else
            {
                DisableOutline(); // Disable outline when not hovered
            }
        }
        else
        {
            DisableOutline(); // Disable outline if no object is hit
        }
    }

    private void EnableOutline()
    {
        if (objectRenderer != null && outlineMaterial != null)
        {
            objectRenderer.material = outlineMaterial;
        }
    }

    private void DisableOutline()
    {
        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }
}