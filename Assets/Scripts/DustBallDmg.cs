using UnityEngine;
using System.Collections;

public class DustBallDmg : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Dust Bunny caught the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();
        }
    }
}
