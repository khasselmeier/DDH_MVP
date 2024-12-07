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
    public int amount = 1;

    private bool isPlayerInRange = false;
    private PlayerBehavior player;

    [Header("UI Elements")]
    public GameObject pickupPanel;

    [Header("Outline Effect")]
    public Material outlineMaterial;
    public Material originalMaterial;
    private Renderer objectRenderer;

    private void Start()
    {
        if (itemType == ItemType.Rock)
        {
            amount = Random.Range(1, 10); // random value for rocks
        }
        else if (itemType == ItemType.Gold)
        {
            amount = Random.Range(3, 12); // random value for gold
        }

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(false);
        }

        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material; // store the original material
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.GetComponent<PlayerBehavior>();

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
            player = null;

            if (pickupPanel != null)
            {
                pickupPanel.SetActive(false);
            }

            DisableOutline(); // remove outline when out of range
        }
    }

    private void Update()
    {
        // Handle item pickup when the player presses 'F'
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (player != null)
            {
                // Handle the pickup logic
                switch (itemType)
                {
                    case ItemType.Rock:
                        player.AddAmmo(amount); // add rocks to player's inventory
                        SoundController.instance.PlayRockPickupSound(); // play rock pickup sound
                        break;
                    case ItemType.Gold:
                        player.AddGold(amount); // add gold to player's inventory
                        SoundController.instance.PlayGoldPickupSound(); // play gold pickup sound
                        break;
                }

                // update the UI
                GameUI.instance.UpdateAmmoText();
                GameUI.instance.UpdateGoldText(player.gold);

                // hide the pickup panel and destroy the item
                if (pickupPanel != null)
                {
                    pickupPanel.SetActive(false);
                }

                Destroy(gameObject);
            }
        }

        CheckForHover();
    }

    private void CheckForHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                EnableOutline();
            }
            else
            {
                DisableOutline();
            }
        }
        else
        {
            DisableOutline();
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