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
            Debug.LogError("Player reference is missing.");
        }
    }

    /*private IEnumerator DelayedInitialization()
    {
        while (player == null)
        {
            player = FindObjectOfType<PlayerBehavior>();
            if (player == null)
            {
                Debug.Log("Waiting for PlayerBehavior...");
                yield return new WaitForSeconds(0.1f); // recheck for playerbehavior every 0.1 seconds
            }
        }

        Initialize();
        Debug.Log("PlayerBehavior found and initialized.");
    }

    private void Start()
    {
        StartCoroutine(DelayedInitialization());
    }

    /*private void Start()
    {
        player = FindObjectOfType<PlayerBehavior>();
        if (player != null)
        {
            Initialize();
        }
        else
        {
            Debug.LogError("PlayerBehavior not found in the scene.");
        }
    }*/

    /*public void Initialize()
    {
        if (player != null && player.rocks != null)
        {
            UpdateAmmoText();
        }
        //UpdateAmmoText();
        UpdateGoldText(player.gold);
        UpdateTotalGemsText(); // update "total quota to win" UI on initialization
        UpdateHealthText(player.currentHealth, player.maxHealth);
        UpdateGemsValueText(0);
    }*/

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
            Debug.LogError("Player or player's rocks is not initialized.");
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