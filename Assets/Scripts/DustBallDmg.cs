using UnityEngine;
using System.Collections;

public class DustBallDmg : MonoBehaviour
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

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Dust Ball hit the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();
        }
    }
}
