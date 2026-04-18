using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class HostileAI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header ("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool haspatrolPoint;

    [Header("Combat Settings")]
    [SerializeField] public float speed = 5f;
    [SerializeField] private float attackCooldown=1f;
    private bool isOnAttackCooldown;
    [SerializeField] private float forwardShotForce = 30f;
    [SerializeField] private float verticalShotForce = 15f;

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;

    private bool isPlayerVisible;
    private bool isPlayerInRange;

    private Rigidbody rb;
 
    public float jumpForce = 1000f;
    public float hopInterval = 2f;
    public AudioSource audioSource;
    public AudioClip attackClip;
    private void Awake()
    {
        // Debug.Log("Starting Dustbunny");
        if(playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if(playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.speed = 10f; 
        }
    }  
    private void Update()
    {
        DetectPlayer();
        UpdateBehaviourState();
        transform.LookAt(playerTransform);
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

        // Check if player is within engagement range
        isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
    }

    private void FireProjectile()
    {
        Debug.Log("bunny fires!");
        if (projectilePrefab == null || firepoint == null) return;
        
        // Aim at the player's actual position
        Vector3 targetPoint = playerTransform.position;

        // Rotate fire point fully toward player, including up/down
        Vector3 shotDirection = (targetPoint - firepoint.position).normalized;
        if (shotDirection != Vector3.zero)
        {
            firepoint.rotation = Quaternion.LookRotation(shotDirection);
        }

        // Instantiate projectile and apply force
        Rigidbody projectileRb = Instantiate(projectilePrefab, firepoint.position, firepoint.rotation).GetComponent<Rigidbody>();
        projectileRb.AddForce(transform.forward * forwardShotForce);
        projectileRb.AddForce(transform.up * verticalShotForce);

        Destroy(projectileRb.gameObject, 3f);
    }

    private void FindPatrolPoint()
    {
       float randomX = Random.Range(-patrolRadius, patrolRadius);
       float randomZ = Random.Range(-patrolRadius, patrolRadius);
       Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Raycast down from above the potential point to find the ground (using terrainLayer)
        if(Physics.Raycast(potentialPoint,-transform.up, 2f, terrainLayer))
        {
            currentPatrolPoint = potentialPoint;
            haspatrolPoint = true;
        }
    }

    private IEnumerator AttackcooldownRoutine()
    {
        isOnAttackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnAttackCooldown = false;
    }

    private void PerformPatrol()
    {
        if (!haspatrolPoint) FindPatrolPoint();

        // move to patrol point
        if (haspatrolPoint) {
            // Debug.Log("bunny patrols ");
            // Debug.Log(currentPatrolPoint);
            // navAgent.SetDestination(currentPatrolPoint);
            float step =  speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, currentPatrolPoint, step);
        }

        // if reached patrol point
        if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
        {    
            haspatrolPoint = false;
            FindPatrolPoint(); // generate a new one
        }
    
    }
    IEnumerator HopRoutine()
    {
        while (true)
        {
            // Apply upward force for hop
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(hopInterval);
        }
    }

    IEnumerator PerformPatrolRoutine() {
        while (true) 
        {
            HopRoutine();    
        }
    }



    private void PerformChase()
    {
        if(playerTransform != null) {
            // navAgent.SetDestination(playerTransform.position);
            float step =  speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
            GameObject playerObj = GameObject.FindWithTag("Player");
            playerTransform = playerObj.transform;
            transform.LookAt(playerTransform);
        }
    // Debug.Log("bunny chases");
    // Debug.Log(playerTransform.position);
    }

    private void PerformAttack()
    {
        float step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
        GameObject playerObj = GameObject.FindWithTag("Player");
        playerTransform = playerObj.transform;
        transform.LookAt(playerTransform);

        if(playerTransform != null)
        {
            transform.LookAt(playerTransform);
        }

        if(!isOnAttackCooldown)
        {
            audioSource.PlayOneShot(attackClip);
            FireProjectile();
            StartCoroutine(AttackcooldownRoutine());
        }
    }

    private void UpdateBehaviourState()
    {
        if(!isPlayerVisible && !isPlayerInRange)
        {
            PerformPatrol();
        }

        else if (isPlayerVisible && !isPlayerInRange)
        {
            PerformChase();
        }

        else if (isPlayerVisible && isPlayerInRange)
        {
            PerformAttack();
        }

    }

}