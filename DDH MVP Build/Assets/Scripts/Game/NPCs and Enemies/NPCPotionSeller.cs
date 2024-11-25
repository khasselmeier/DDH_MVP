using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCPotionSeller : MonoBehaviour
{
    [Header("Potion Trade Settings")]
    public int potionCost = 50; // price of potion
    public int potionHealth = 20; // health restored=

    [Header("UI Elements")]
    public TextMeshProUGUI tradePromptText;
    public GameObject tradePanel;

    private bool isPlayerInRange = false;
    private PlayerBehavior player;

    private void Start()
    {
        tradePanel.SetActive(false);
        tradePromptText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.GetComponent<PlayerBehavior>();

            tradePromptText.gameObject.SetActive(true);
            tradePromptText.text = "Hurt? Trade 50 gold for an instant health potion";
            tradePanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;

            tradePanel.SetActive(false);
            tradePromptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TradePotion();
        }
    }

    private void TradePotion()
    {
        if (player == null)
            return;

        if (player.gold >= potionCost)
        {
            player.gold -= potionCost;
            player.Heal(potionHealth);

            // update the UI
            GameUI.instance.UpdateGoldText(player.gold);
            GameUI.instance.UpdateHealthText(player.currentHealth, player.maxHealth);

            //Debug.Log("Potion purchased! Health restored by 20");
        }
        else
        {
            //Debug.Log("Not enough gold for trade");
        }
    }
}