using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // LOAD GAME FROM MENU\
    public AudioSource backgroundMusic;
    public void PlayGame()
    {
        Time.timeScale = 1f; // just in case game was paused
        SceneManager.LoadScene("SampleScene");
        if (backgroundMusic != null)
            backgroundMusic.Play();
    }

    // RESTART CURRENT LEVEL
    public void RestartGame()
    {
        Time.timeScale = 1f; // reset time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (backgroundMusic != null)
            backgroundMusic.Play();
    }
}