using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteUI;

    public AudioSource audioSource;
    public AudioClip levelCompleteClip;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            audioSource.PlayOneShot(levelCompleteClip);
            levelCompleteUI.SetActive(true);
            Time.timeScale = 0f;
            Destroy(gameObject);
        }
    }
}