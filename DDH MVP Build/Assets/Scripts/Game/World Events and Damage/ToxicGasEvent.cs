using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToxicGasEvent : MonoBehaviour
{
    public float gasChance = 0.1f; // 10% chance for toxic gas to occur per check
    public float gasDuration = 10f;
    public float gasDamageInterval = 1f; // how often the player takes damage per second
    public int damagePerTick = 5;
    public Image gasScreenOverlay;
    public TMP_Text gasNotificationText; // UI text for gas notification

    private bool isGassing = false;
    private float timeSinceLastCheck = 0f;
    public float checkInterval = 30f;

    private PlayerBehavior player;
    public float fadeSpeed = 1f;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // start looking for the player in a coroutine if it's not assigned immediately
        StartCoroutine(FindPlayerCoroutine());

        if (gasNotificationText == null)
        {
            //Debug.LogError("Gas notification text not assigned");
        }
        else
        {
            gasNotificationText.gameObject.SetActive(false); // hide at the start
        }
    }

    private void Update()
    {
        if (!isGassing && player != null)
        {
            // check periodically if toxic gas should start
            timeSinceLastCheck += Time.deltaTime;
            if (timeSinceLastCheck >= checkInterval)
            {
                timeSinceLastCheck = 0f;
                TryStartGas();
            }
        }
    }

    private IEnumerator FindPlayerCoroutine()
    {
        // wait until the PlayerBehavior is available
        while (player == null)
        {
            player = FindObjectOfType<PlayerBehavior>();
            if (player == null)
            {
                //Debug.Log("Waiting for PlayerBehavior to be assigned...");
                yield return new WaitForSeconds(0.5f); // try again every 0.5 seconds
            }
        }
        //Debug.Log("PlayerBehavior found and assigned");
    }

    void TryStartGas()
    {
        // random chance to start a toxic gas event
        if (Random.value <= gasChance)
        {
            StartGas();
        }
    }

    void StartGas()
    {
        Debug.Log("Toxic gas event has started");
        isGassing = true;
        gasNotificationText.gameObject.SetActive(true); // show the gas notification text
        gasNotificationText.text = "Toxic gas seems to be leaking";

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeScreen(true)); // fade the screen to green

        StartCoroutine(GasCoroutine());
    }

    IEnumerator GasCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < gasDuration)
        {
            if (player != null)
            {
                player.TakeDamage(damagePerTick); // deal damage to the player
                //Debug.Log($"Player took {damagePerTick} damage from toxic gas");
            }

            // wait for the damage interval before dealing damage again
            yield return new WaitForSeconds(gasDamageInterval);
            elapsedTime += gasDamageInterval;
        }

        EndGas();
    }

    void EndGas()
    {
        Debug.Log("Toxic gas event has ended");
        isGassing = false;
        gasNotificationText.gameObject.SetActive(false); // hide the gas notification text
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeScreen(false)); // fade the screen back to normal
    }

    IEnumerator FadeScreen(bool fadeIn)
    {
        Color overlayColor = gasScreenOverlay.color;
        float targetAlpha = fadeIn ? 0.4f : 0f; // 40% green when the toxic gas is active, 0% when it's over

        while (!Mathf.Approximately(overlayColor.a, targetAlpha))
        {
            overlayColor.a = Mathf.MoveTowards(overlayColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            gasScreenOverlay.color = overlayColor;
            yield return null;
        }
    }
}