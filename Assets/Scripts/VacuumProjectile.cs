using UnityEngine;

public class SentryProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    //TODO: 50/50 take life or phase through ground

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Projectile collided with: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Projectile hit the player!");

            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.LoseLife();
                Debug.Log("Life taken from player.");
            }
            else
            {
                Debug.Log("PlayerMovement script not found on collided player object.");
            }

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}