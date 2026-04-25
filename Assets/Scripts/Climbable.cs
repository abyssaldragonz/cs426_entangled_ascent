using UnityEngine;

public class Climbable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Scratchpost: collision with player detected");
            var playerRB = other.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = transform.position - other.transform.position;
            direction.Normalize();
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);
            playerRB.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}