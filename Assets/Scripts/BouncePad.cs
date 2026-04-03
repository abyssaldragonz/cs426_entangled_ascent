using UnityEngine;

public class BouncePad : MonoBehaviour
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
        var otherRB = other.gameObject.GetComponent<Rigidbody>();
        Debug.Log("Bounce Pad: collision with player detected");
        Vector3 direction = transform.position - other.transform.position;
        direction.Normalize();
        otherRB.linearVelocity = new Vector3(otherRB.linearVelocity.x, 0, otherRB.linearVelocity.z);
        otherRB.AddForce(Vector3.up * 20f, ForceMode.Impulse);
    }
}