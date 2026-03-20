using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelCompleteUI.SetActive(true);
            Time.timeScale = 0f;
            Destroy(gameObject);
        }
    }
}