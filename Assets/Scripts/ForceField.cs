using UnityEngine;

public class ForceField : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Force Field: collision with player detected");
            var playerRB = other.gameObject.GetComponent<Rigidbody>();
            // playerRB.linearVelocity = new Vector3(10f, 15f, 10f);
            // playerRB.linearVelocity -= other.transform.forward * 50f;
            // playerRB.linearVelocity += other.transform.up * 5f;
            Vector3 direction = transform.position - other.transform.position;
            direction.Normalize();
            direction = Vector3.Reflect(playerRB.linearVelocity, other.contacts[0].normal);
            // gameObject.GetComponent<Rigidbody>().AddForce(direction * 5000, ForceMode.Impulse);
            // Debug.Log($"Direction! " + direction);
        }
    }
}