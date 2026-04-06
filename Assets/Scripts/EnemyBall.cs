using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;  // Destroy after 5 seconds

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * -speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detect player hit
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            // Add damage logic here if needed
        }

        // Bounce off surfaces
    }
}