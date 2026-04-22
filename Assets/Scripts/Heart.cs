using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        playerTransform = playerObj.transform;
        transform.LookAt(playerTransform);
        transform.LookAt(playerTransform);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            var player = other.gameObject.GetComponent<PlayerMovement>();
            if (player.catLives < 9) { 
                player.GainLife();
                Destroy(gameObject);
            }

        }
    }
}