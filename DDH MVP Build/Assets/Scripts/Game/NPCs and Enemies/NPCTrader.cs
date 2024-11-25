using UnityEngine;
using TMPro;

public class NPCTrader : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int upgradeCostBaseGem = 100;
    public int upgradeCostHighGem = 200;

    [Header("UI Elements")]
    public TextMeshProUGUI goldNeededTextFirst;
    public TextMeshProUGUI goldNeededTextSecond;
    public TextMeshProUGUI tradePromptText;
    public GameObject tradePanel; // UI panel to show interact button when in range

    private PlayerBehavior player;
    private bool isPlayerInRange = false;
    private bool hasTradedBaseGem = false;
    private bool hasTradedHighGem = false;

    private void Start()
    {
        goldNeededTextSecond.gameObject.SetActive(false); //set second trade text to false (hide)
        goldNeededTextFirst.gameObject.SetActive(true); //set first trade text to true (show)
        goldNeededTextFirst.text = "100 Gold Needed for a Pickaxe";

        //hide UI elements
        tradePromptText.gameObject.SetActive(false);
        tradePanel.SetActive(false); // hide at the start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerBehavior>();
            isPlayerInRange = true;

            tradePanel.SetActive(true); // show UI panel

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
            tradePanel.SetActive(false);

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
            tradePromptText.text = "Having trouble mining some gems? I can fix that for 200 gold";

            goldNeededTextFirst.gameObject.SetActive(false); //set first trade text to false after first trade
            goldNeededTextSecond.gameObject.SetActive(true); //set second trade text to true after first trade
            goldNeededTextSecond.text = "200 Gold Needed for Upgrade";
        }
        // trade for high gems
        else if (hasTradedBaseGem && !hasTradedHighGem && player.gold >= upgradeCostHighGem)
        {
            player.gold -= upgradeCostHighGem;
            player.canMineHighGem = true;
            hasTradedHighGem = true;
            player.hasTraded = true;
            tradePromptText.gameObject.SetActive(false);

            //Debug.Log("Player traded for ability to mine high gems");

            goldNeededTextSecond.gameObject.SetActive(false); //set second trade text to false after second trade
        }

        GameUI.instance.UpdateGoldText(player.gold); // update gold UI
    }
}