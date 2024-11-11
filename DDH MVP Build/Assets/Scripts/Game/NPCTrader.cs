using UnityEngine;
using TMPro;

public class NPCTrader : MonoBehaviour
{
    public int upgradeCostBaseGem = 50;
    public int upgradeCostHighGem = 100;
    public int damageIncrease = 5;
    public TextMeshProUGUI tradePromptText;
    public GameObject tradePanel; // UI panel to show interact button when in range

    private PlayerBehavior player;
    private bool isPlayerInRange = false;
    private bool hasTradedBaseGem = false;
    private bool hasTradedHighGem = false;

    private void Start()
    {
        // trade prompt is hidden at the start
        tradePromptText.gameObject.SetActive(false);
        if (tradePanel != null)
        {
            tradePanel.SetActive(false); // hide at the start
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerBehavior>();
            isPlayerInRange = true;

            if (tradePanel != null)
            {
                tradePanel.SetActive(true); // show UI panel
            }

            // update trade prompt based on player's trade status
            if (!hasTradedBaseGem)
            {
                tradePromptText.gameObject.SetActive(true);
                tradePromptText.text = "Trade 100 gold for a pickaxe to mine gems";
            }
            else if (hasTradedBaseGem && !hasTradedHighGem)
            {
                tradePromptText.gameObject.SetActive(true);
                tradePromptText.text = "Having trouble mining some gems? I can fix that for 200 gold";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;

            // hide panel when the player leaves the range
            if (tradePanel != null)
            {
                tradePanel.SetActive(false);
            }

            // hide trade prompt UI when the player leaves the range
            tradePromptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && player != null && Input.GetKeyDown(KeyCode.F))
        {
            TradeWithPlayer(player);
        }
    }

    private void TradeWithPlayer(PlayerBehavior player)
    {
        // trade for base gems
        if (!hasTradedBaseGem && player.gold >= upgradeCostBaseGem)
        {
            player.gold -= upgradeCostBaseGem;
            player.canMineBaseGem = true;
            hasTradedBaseGem = true;
            player.hasTraded = true;
            player.PerformTrade();  // call PerformTrade() to activate pickaxe after first trade
            tradePromptText.gameObject.SetActive(false);

            //Debug.Log("Player traded for ability to mine base gems");

            // update the NPC UI text
            tradePromptText.gameObject.SetActive(true);
            tradePromptText.text = "Having trouble mining some gems? I can fix that for 100 gold";
        }
        // trade for high gems
        else if (hasTradedBaseGem && !hasTradedHighGem && player.gold >= upgradeCostHighGem)
        {
            player.gold -= upgradeCostHighGem;
            player.canMineHighGem = true;
            hasTradedHighGem = true;
            player.hasTraded = true;
            //player.PerformTrade();
            tradePromptText.gameObject.SetActive(false);

            //Debug.Log("Player traded for ability to mine high gems");
        }
        else
        {
            //Debug.Log("Not enough gold for the upgrade");
        }

        GameUI.instance.UpdateGoldText(player.gold); // update gold UI
    }
}