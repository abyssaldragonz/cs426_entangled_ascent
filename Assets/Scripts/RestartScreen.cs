using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScreen : MonoBehaviour
{

    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        restartButton.onClick.AddListener(restartScene);
        quitButton.onClick.AddListener(quitGame);
    }

    
    private void restartScene()
    {
        Debug.Log("Restarting game!");
        SceneManager.LoadScene("GameScene");
    }

    private void quitGame()
    {
        Debug.Log("QUITTING game!");
        Application.Quit();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
