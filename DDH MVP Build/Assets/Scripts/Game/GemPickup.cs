using UnityEngine;

public class GemPickup : MonoBehaviour
{
    public static int totalGems;
    public static int collectedGems;

    private bool isPlayerInRange = false;
    private PlayerBehavior player;

    [Header("Gem Value Settings")]
    public int minGemValue = 10;
    public int maxGemValue = 100;
    public int gemValue;

    private void Start()
    {
        totalGems = Random.Range(60, 150); // quota will be between the value
        //Debug.Log("Total quota needed to win: " + totalGems);

        // assign appropriate value range based on tag
        if (CompareTag("BaseGem"))
        {
            minGemValue = 1;
            maxGemValue = 30;
        }
        else if (CompareTag("HighGem"))
        {
            minGemValue = 30;
            maxGemValue = 60;
        }

        gemValue = Random.Range(minGemValue, maxGemValue + 1);
        //Debug.Log($"{gameObject.tag} value assigned: " + gemValue);

        // Update UI with the total gems (quota needed to win)
        if (GameUI.instance != null)
        {
            GameUI.instance.UpdateTotalGemsText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered gem pickup range.");
            isPlayerInRange = true;
            player = other.GetComponent<PlayerBehavior>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player exited gem pickup range.");
            isPlayerInRange = false;
            player = null;
        }
    }
}