using UnityEngine;

public class GemProximityUI : MonoBehaviour
{
    public GameObject gemUIPanel;
    private PlayerBehavior playerBehavior; // ref to PlayerBehavior script

    private void Start()
    {
        if (gemUIPanel != null)
        {
            gemUIPanel.SetActive(false); // hide the panel initially
        }
    }

    private void Update()
    {
        // continuously search for the PlayerBehavior script until found
        if (playerBehavior == null)
        {
            playerBehavior = FindObjectOfType<PlayerBehavior>();

            if (playerBehavior == null)
            {
                //Debug.Log("Searching for PlayerBehavior...");
                return;
            }
        }

        // check if the player has traded at least once and is near a gem
        if (playerBehavior.hasTraded && playerBehavior.IsNearGem())
        {
            ShowUIPanel();
        }
        else
        {
            HideUIPanel();
        }
    }

    private void ShowUIPanel()
    {
        if (gemUIPanel != null && !gemUIPanel.activeSelf)
        {
            gemUIPanel.SetActive(true);
        }
    }

    private void HideUIPanel()
    {
        if (gemUIPanel != null && gemUIPanel.activeSelf)
        {
            gemUIPanel.SetActive(false);
        }
    }
}