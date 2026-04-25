using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuCam : MonoBehaviour
{
    public GameObject controlsPopup;

    public void PlayGame()
    {
        controlsPopup.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
