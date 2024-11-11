using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) // only have one instance of the game manager
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        PlayerBehavior player = FindObjectOfType<PlayerBehavior>();

        if (player != null && player.totalValueOfGems > GemPickup.totalGems)
        {
            WinGame();
            //Debug.Log("Win condition met! Player's Total Value: " + player.totalValueOfGems + " exceeds " + GemPickup.totalGems);
        }
    }

    public void WinGame()
    {
        //Debug.Log("You collected the quota! You win!");
        SceneManager.LoadScene("End"); // switch to "end" scene
    }

    public void LoseGame()
    {
        //Debug.Log("Game Over. You Lose");
        SceneManager.LoadScene("Lose"); // switch to "Lose" scene
    }
}
