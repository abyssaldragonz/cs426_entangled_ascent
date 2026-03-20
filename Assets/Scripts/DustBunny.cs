using UnityEngine;

public class DustBunny : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ========== Collided with Player ===================================
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Dust Bunny caught the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();
        }
    }
}
