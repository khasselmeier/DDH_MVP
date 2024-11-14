using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemsValueText; // quota collected
    public TextMeshProUGUI totalGemsText; // quota to reach
    public TextMeshProUGUI healthText;

    private PlayerBehavior player;

    //instance
    public static GameUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerBehavior.PlayerInitialized += OnPlayerInitialized;
    }

    private void OnDisable()
    {
        PlayerBehavior.PlayerInitialized -= OnPlayerInitialized;
    }

    private void OnPlayerInitialized(PlayerBehavior player)
    {
        this.player = player;
        Initialize();
    }

    public void Initialize()
    {
        if (player != null)
        {
            UpdateHealthText(player.currentHealth, player.maxHealth);
            UpdateGoldText(player.gold);
            UpdateTotalGemsText();
            UpdateAmmoText();
        }
        else
        {
            //Debug.LogError("Player reference is missing");
        }
    }

    public void UpdateTotalGemsText()
    {
        totalGemsText.text = "Quota to Win: " + GemPickup.totalGems;
    }

    public void UpdateAmmoText()
    {
        if (player != null && player.rocks != null)
        {
            //Debug.Log("Update Ammo UI: " + player.rocks.curAmmo + " / " + player.rocks.maxAmmo);
            ammoText.text = " " + player.rocks.curAmmo + " / " + player.rocks.maxAmmo;
        }
        else
        {
            //Debug.LogError("Player or player's rocks is not initialized");
        }
    }

    public void UpdateGemsValueText(int totalValue)
    {
        gemsValueText.text = " " + totalValue;
    }

    public void UpdateGoldText(int goldAmount)
    {
        goldText.text = " " + goldAmount;
    }

    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = " " + currentHealth + " / " + maxHealth;
    }
}