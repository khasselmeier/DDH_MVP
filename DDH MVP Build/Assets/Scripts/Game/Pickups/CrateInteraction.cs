using UnityEngine;

public class CrateInteraction : MonoBehaviour
{
    [Header("Outline Settings")]
    public Material outlineMaterial;
    private Material originalMaterial;
    private Renderer crateRenderer;

    [Header("Pickup Settings")]
    public int minRocks = 5;
    public int maxRocks = 20;

    public int minGold = 10;
    public int maxGold = 50;

    public int minHealth = 10;
    public int maxHealth = 30;

    [Header("UI Elements")]
    public GameObject pickupPanel;

    private bool isPlayerInRange = false;
    private bool isInteracted = false; // prevents multiple interactions with the same crate
    private PlayerBehavior player;

    private void Start()
    {
        crateRenderer = GetComponent<Renderer>();
        if (crateRenderer != null)
        {
            originalMaterial = crateRenderer.material;
        }

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracted)
        {
            isPlayerInRange = true;
            player = other.GetComponent<PlayerBehavior>();

            // show the panel and update text
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

            // hide the panel
            if (pickupPanel != null)
            {
                pickupPanel.SetActive(false);
            }

            RemoveOutline();
        }
    }

    private void Update()
    {
        HandleHoverEffect();

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !isInteracted)
        {
            isInteracted = true;
            GiveRandomReward(player);

            // hide the panel after interaction
            if (pickupPanel != null)
            {
                pickupPanel.SetActive(false);
            }
        }
    }


    private void HandleHoverEffect()
    {
        // raycast to detect if the player is looking at the crate
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject && !isInteracted)
            {
                ApplyOutline();
            }
            else
            {
                RemoveOutline();
            }
        }
    }

    private void ApplyOutline()
    {
        if (crateRenderer != null && outlineMaterial != null)
        {
            crateRenderer.material = outlineMaterial;
        }
    }

    private void RemoveOutline()
    {
        if (crateRenderer != null && originalMaterial != null)
        {
            crateRenderer.material = originalMaterial;
        }
    }

    private void GiveRandomReward(PlayerBehavior player)
    {
        if (player == null) return;

        int rewardType = Random.Range(0, 3);

        switch (rewardType)
        {
            case 0:
                int rocks = Random.Range(minRocks, maxRocks + 1);
                player.AddAmmo(rocks);
                GameUI.instance.UpdateAmmoText();
                SoundController.instance.PlayRockPickupSound();
                break;

            case 1:
                int gold = Random.Range(minGold, maxGold + 1);
                player.AddGold(gold);
                GameUI.instance.UpdateGoldText(player.gold);
                SoundController.instance.PlayGoldPickupSound();
                break;

            case 2:
                int health = Random.Range(minHealth, maxHealth + 1);
                player.Heal(health);
                GameUI.instance.UpdateHealthText(player.currentHealth, player.maxHealth);
                SoundController.instance.PlayHealthPickupSound();
                break;
        }
    }
}