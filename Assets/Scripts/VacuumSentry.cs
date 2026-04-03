using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumSentry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ========== Observed Player ========================================
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            int sentryChoice = UnityEngine.Random.Range(0,2); 
            Debug.Log("Player observed by sentry! Choosing option: " + sentryChoice);

            switch (sentryChoice) {
                case 0: // freeze player
                    FreezePlayer(other.gameObject);
                    break;

                case 1: // phase through the ground to previous floor
                    // to be implemented
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UnfreezePlayer(other.gameObject);
        }
    }


    // ========== Freezing Player ========================================
    private void FreezePlayer(GameObject player)
    {
        Debug.Log("Spotlight: collision with player detected");
        var playerRB = player.GetComponent<Rigidbody>();
        // stop player from moving
        player.GameObject().GetComponent<PlayerMovement>().enabled = false; 
        // player.GameObject().SetActive(false);
        playerRB.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void UnfreezePlayer(GameObject player)
    {
        var playerRB = player.GetComponent<Rigidbody>();
        // let player move
        player.GameObject().GetComponent<PlayerMovement>().enabled = true; 
        // player.GameObject().SetActive(true);
        playerRB.constraints = RigidbodyConstraints.None;
        playerRB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // ========== Phasing Player =========================================
    private void PhasingPlayer(GameObject player)
    {
        // TODO
    }
}