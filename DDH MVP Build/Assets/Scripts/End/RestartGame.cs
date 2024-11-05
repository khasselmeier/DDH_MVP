using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Start()
    {
        //enable the cursor since we hide it when we play the game
        Cursor.lockState = CursorLockMode.None;
    }
    public void RestartGameButton()
    {
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene("Menu"); // switch to "Menu" scene
    }
}