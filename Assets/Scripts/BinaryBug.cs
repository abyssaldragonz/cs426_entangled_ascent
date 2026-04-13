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
    [SerializeField] private float damageCooldown = 1.5f;
    private bool canDealDamage = true;

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

    // ANIMATIONS AND SOUND
    private Animator anim;
    public AudioSource audioSource;
    public AudioClip idleClip;
    public AudioClip attackClip;
 
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
        anim.ResetTrigger("DetectPlayer");
        GameObject playerObj = GameObject.FindWithTag("Player");
        playerTransform = playerObj.transform;
        playerPositions.Insert(0, playerTransform.position); // add the next player position

        speed = 3f; // slow down
        float step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, playerPositions[0], step);
        
        playerPositions.RemoveAt(0); // remove first node 
        Debug.Log("Bug following scent...");
        // Debug.Log("Player positions:");
        // foreach( var x in playerPositions) {
        //     Debug.Log( x.ToString());
        // }
    }


    // beeline straight towards the player
    private void PerformChase(float newSpeed)
    {
        anim.SetTrigger("DetectPlayer");
        speed = newSpeed;
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
            PerformChase(3f);
        }
        else if (isPlayerVisible && !isPlayerInRange)
        {
            anim.ResetTrigger("AttackMode"); // no longer in attack animation
            playerPositions.Clear(); // empty out the scent
            PerformChase(5f); // and chase
        }

        else if (isPlayerInRange) // do attack
        {
            anim.SetTrigger("AttackMode");
            PerformChase(5f);
            audioSource.PlayOneShot(attackClip);
        }

        // bug sees player so it marked the scent of player
        if (isPlayerVisible)
        {
            detectedPlayer = true; 
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && canDealDamage)
        {
            Debug.Log("Binary Bug hit the player!");

            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.LoseLife();
            }

            anim.SetTrigger("AttackMode");
            StartCoroutine(DamageCooldownRoutine());
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDealDamage = true;
    }
}