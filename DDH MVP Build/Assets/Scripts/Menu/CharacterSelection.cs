using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public TextMeshProUGUI messageText;  // displayes messages
    public GameObject confirmationPanel; // UI Panel for confirmation (with Yes/No buttons)
    public TextMeshProUGUI confirmationText; // text in the confirmation panel to show the chosen character

    private string selectedCharacter;  // stores the player's selected character

    public void ChooseCharOne()
    {
        selectedCharacter = "Moldrock";
        ShowConfirmation();
    }

    public void ChooseCharTwo()
    {
        selectedCharacter = "Thalgrim";
        ShowConfirmation();
    }

    // confirmation dialog
    private void ShowConfirmation()
    {
        confirmationText.text = $"Are you sure you want to choose {selectedCharacter}?";
        confirmationPanel.SetActive(true);
    }

    // called when the player confirms their selection
    public void ConfirmSelection()
    {
        messageText.text = $"You have chosen {selectedCharacter}!";
        confirmationPanel.SetActive(false);
        //Debug.Log($"{selectedCharacter} selected");

        // save the selected character choice and load the game scene
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacter);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // called when the player cancels their selection
    public void CancelSelection()
    {
        confirmationPanel.SetActive(false); // hide the confirmation panel
        messageText.text = "Selection canceled. Please choose a character.";
        selectedCharacter = string.Empty;  // clear selected character
    }
}
