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
        transform.LookAt(playerTransform);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            var player = other.gameObject.GetComponent<PlayerMovement>();
            player.GainLife();
            Destroy(gameObject);
        }
    }
}