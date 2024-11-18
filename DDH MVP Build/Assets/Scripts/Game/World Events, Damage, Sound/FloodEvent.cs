using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloodEvent : MonoBehaviour
{
    public float floodChance = 0.1f; // 10% chance for a flood to occur per check
    public float floodDuration = 10f;
    public float floodDamageInterval = 1f; // how often the player takes damage per second
    public int damagePerTick = 5;
    public Image floodScreenOverlay;
    public TMP_Text floodNotificationText; //UI text for flood notif

    private bool isFlooding = false;
    private float timeSinceLastCheck = 0f;
    public float checkInterval = 30f;

    private PlayerBehavior player;
    public float fadeSpeed = 1f;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Start looking for the player in a coroutine if it's not assigned immediately
        StartCoroutine(FindPlayerCoroutine());

        if (floodNotificationText == null)
        {
            //Debug.LogError("Flood notification text not assigned.");
        }
        else
        {
            floodNotificationText.gameObject.SetActive(false); // hide at the start
        }
    }

    private void Update()
    {
        if (!isFlooding && player != null)
        {
            // check periodically if a flood should start
            timeSinceLastCheck += Time.deltaTime;
            if (timeSinceLastCheck >= checkInterval)
            {
                timeSinceLastCheck = 0f;
                TryStartFlood();
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
        //Debug.Log("PlayerBehavior found and assigned.");
    }

    void TryStartFlood()
    {
        //random chance to start a flood
        if (Random.value <= floodChance)
        {
            StartFlood();
        }
    }

    void StartFlood()
    {
        Debug.Log("Flood has started");
        isFlooding = true;
        floodNotificationText.gameObject.SetActive(true); //show the flood notification text
        floodNotificationText.text = "Water seems to be rapidly increasing";

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeScreen(true)); //fade the screen to blue

        StartCoroutine(FloodCoroutine());
    }

    IEnumerator FloodCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < floodDuration)
        {
            if (player != null)
            {
                player.TakeDamage(damagePerTick); //deal dmg to the player
                //Debug.Log($"Player took {damagePerTick} damage from flooding.");
            }

            // wait for the dmg interval before dealing dmg again
            yield return new WaitForSeconds(floodDamageInterval);
            elapsedTime += floodDamageInterval;
        }

        EndFlood();
    }

    void EndFlood()
    {
        Debug.Log("The flood has ended");
        isFlooding = false;
        floodNotificationText.gameObject.SetActive(false);
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeScreen(false));
    }

    IEnumerator FadeScreen(bool fadeIn)
    {
        Color overlayColor = floodScreenOverlay.color;
        float targetAlpha = fadeIn ? 0.3f : 0f; // 50% blue when the flood is active, 0% when it's over

        while (!Mathf.Approximately(overlayColor.a, targetAlpha))
        {
            overlayColor.a = Mathf.MoveTowards(overlayColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            floodScreenOverlay.color = overlayColor;
            yield return null;
        }
    }
}