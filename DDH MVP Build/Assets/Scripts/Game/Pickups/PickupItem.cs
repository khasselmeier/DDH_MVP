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
    public int amount = 1; // amt to add to player's inventory

    private bool isPlayerInRange = false;
    private PlayerBehavior player; // ref to the player

    [Header("UI Elements")]
    public GameObject pickupPanel; // UI panel to show interact button when in pickup range

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
            pickupPanel.SetActive(false); // hide the UI panel initially
        }

        //Debug.Log($"{itemType} pickup created with amount: {amount}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.GetComponent<PlayerBehavior>();
            //Debug.Log("Player entered pickup range.");

            // show the panel when the player is in range
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
            player = null; // clears player reference

            // Hide the pickup panel when the player leaves the range
            if (pickupPanel != null)
            {
                pickupPanel.SetActive(false);
            }

            //Debug.Log("Player exited pickup range.");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F)) // Check for 'F' key
        {
            //SoundController.instance.PlayMiningSound(); // play mining SFX

            //Debug.Log("Attempting to collect item...");

            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Rock:
                        player.AddAmmo(amount); // add ammo in PlayerBehavior
                        break;
                    case ItemType.Gold:
                        player.AddGold(amount); // add gold in PlayerBehavior
                        break;
                }

                // Update the UI
                GameUI.instance.UpdateAmmoText(); // update ammo UI
                GameUI.instance.UpdateGoldText(player.gold); // update gold UI

                // hide the panel after
                if (pickupPanel != null)
                {
                    pickupPanel.SetActive(false);
                }

                // destroy item after collection
                Destroy(gameObject);
            }
            else
            {
                //Debug.Log("Player reference is null");
            }
        }
    }
}
