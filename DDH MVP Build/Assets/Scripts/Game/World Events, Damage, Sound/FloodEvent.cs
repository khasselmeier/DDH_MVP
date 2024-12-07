using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloodEvent : MonoBehaviour
{
    public float floodChance = 0.1f;
    public float floodDuration = 10f;
    public float floodDamageInterval = 1f;
    public int damagePerTick = 5;
    public Image floodScreenOverlay;
    public TMP_Text floodNotificationText;
    public TMP_Text areaWarningText;

    private bool isFlooding = false;
    private bool playerInFloodZone = false;
    private float timeSinceLastCheck = 0f;
    public float checkInterval = 30f;

    private PlayerBehavior player;
    public float fadeSpeed = 1f;
    private Coroutine fadeCoroutine;
    private Coroutine floodCoroutine;

    private void Awake()
    {
        StartCoroutine(FindPlayerCoroutine());

        if (floodNotificationText != null)
        {
            floodNotificationText.gameObject.SetActive(false);
        }

        if (areaWarningText != null)
        {
            areaWarningText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInFloodZone && !isFlooding && player != null)
        {
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
        while (player == null)
        {
            player = FindObjectOfType<PlayerBehavior>();
            if (player == null)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void TryStartFlood()
    {
        if (Random.value <= floodChance)
        {
            StartFlood();
        }
    }

    void StartFlood()
    {
        Debug.Log("Flood has started");
        isFlooding = true;
        floodNotificationText.gameObject.SetActive(true);
        floodNotificationText.text = "Water seems to be rapidly increasing";

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeScreen(true));

        floodCoroutine = StartCoroutine(FloodCoroutine());
    }

    IEnumerator FloodCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < floodDuration)
        {
            if (player != null)
            {
                player.TakeDamage(damagePerTick);
            }

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
        float targetAlpha = fadeIn ? 0.3f : 0f;

        while (!Mathf.Approximately(overlayColor.a, targetAlpha))
        {
            overlayColor.a = Mathf.MoveTowards(overlayColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            floodScreenOverlay.color = overlayColor;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInFloodZone = true;
            TryStartFlood();

            if (areaWarningText != null)
            {
                areaWarningText.gameObject.SetActive(true);
                areaWarningText.text = "Warning: Increased flooding in the area";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInFloodZone = false;
            if (floodCoroutine != null)
            {
                StopCoroutine(floodCoroutine);
                EndFlood(); // end the flood immediately when player leaves
            }

            if (areaWarningText != null)
            {
                areaWarningText.gameObject.SetActive(false);
            }
        }
    }
}