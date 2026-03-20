using System;
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
                    // to be implemented
                    break;

                case 1: // phase through the ground to previous floor
                    // to be implemented
                    break;
            }
        }
    }

}
