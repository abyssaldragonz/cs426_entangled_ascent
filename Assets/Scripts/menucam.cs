using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // LOAD GAME FROM MENU
    public void PlayGame()
    {
        Time.timeScale = 1f; // just in case game was paused
        SceneManager.LoadScene("SampleScene");
    }

    // RESTART CURRENT LEVEL
    public void RestartGame()
    {
        Time.timeScale = 1f; // reset time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}