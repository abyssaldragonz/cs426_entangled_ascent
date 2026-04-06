using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System.Collections.Generic;


public class BinaryBug: MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    //[SerializeField] private Transform firePoint;
    //[SerializeField] private GameObject projectilePrefab;

    [Header ("Layers")]
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]

    [Header("Combat Settings")]
    
    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 5f;
    [SerializeField] private float speed = 5f;

    private bool isPlayerVisible;
    private bool isPlayerInRange;
    private bool detectedPlayer;
    private bool runInvoke;

    public List<Vector3> playerPositions = new List<Vector3>();
    private Rigidbody rb;

    // ANIMATIONS
    private Animator anim;
 
    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if(playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        anim = transform.GetChild(0).GetComponent<Animator>();
    }  

    private void Update()
    {
        DetectPlayer();
        UpdateBehaviourState();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw vision range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, engagementRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);      
    }

    private void DetectPlayer()
    {
        // Check if player is within vision range
        isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
        // Debug.Log("Binary Bug sees player!");

        // Check if player is within engagement range
        isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
    }

    // ========== DIGITAL SCENT ==========================================
    void FollowScentTrail()
    {
        // TODO: FIX THIS
        anim.ResetTrigger("DetectPlayer");
        GameObject playerObj = GameObject.FindWithTag("Player");
        playerTransform = playerObj.transform;
        playerPositions.Add(playerTransform.position); // add the next player position

        speed = 3f; // slow down
        float step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, playerPositions[0], step);
        
        playerPositions.RemoveAt(0); // remove first node 
        Debug.Log("Bug following scent...");
        Debug.Log("Player positions:");
        foreach( var x in playerPositions) {
            Debug.Log( x.ToString());
        }
    }


    // beeline straight towards the player
    private void PerformChase()
    {
        anim.SetTrigger("DetectPlayer");
        speed = 5f;
        if (playerTransform != null)
        {
            float step =  speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
            GameObject playerObj = GameObject.FindWithTag("Player");
            playerTransform = playerObj.transform;
            transform.LookAt(playerTransform);
            // Debug.Log(playerTransform.position);
        }
    }

    private void UpdateBehaviourState()
    {
        if (!isPlayerVisible && !isPlayerInRange && detectedPlayer)
        {
            // will now continously go after the player even if out of range
            if (runInvoke == false)
            {
                playerPositions.Add(playerTransform.position);
                InvokeRepeating(nameof(FollowScentTrail), 0.0f, 3f);
                runInvoke = true; // just run this once
            }
        }

        else if (isPlayerVisible && !isPlayerInRange)
        {
            anim.ResetTrigger("AttackMode"); // no longer in attack animation
            playerPositions.Clear(); // empty out the scent
            PerformChase(); // and chase
        }

        else if (isPlayerInRange)
        {
            anim.SetTrigger("AttackMode");
            PerformChase();
        }

        // bug sees player so it marked the scent of player
        if (isPlayerVisible)
        {
            detectedPlayer = true; 
        }
    }

       
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Binary Bug hit the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();

            anim.SetTrigger("AttackMode");
        }
    }
}   