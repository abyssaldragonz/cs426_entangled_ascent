using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;


// ALL LINES OF CODE WITH "//" IN FRONT OF THEM ARE COMMENTED OUT FOR NOW, 
// BUT MAY BE RELEVANT TO FUTURE IMPLEMENTATION OF THE SENTRY'S ABILITIES.
// THEY ARE LEFT IN AS A REFERENCE FOR HOW THE SENTRY'S COMBAT BEHAVIOR MIGHT WORK IN THE FUTURE.

public class BinaryBug: MonoBehaviour
{

    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    //[SerializeField] private Transform firePoint;
    //[SerializeField] private GameObject projectilePrefab;

    [Header ("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool haspatrolPoint;

    [Header("Combat Settings")]
    private bool isOnAttackCooldown;
    
    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;
    [SerializeField] private float speed = 0.25f;

    private bool isPlayerVisible;
    private bool isPlayerInRange;

    private Rigidbody rb;
 
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

        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
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



    private void FindPatrolPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);
        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Raycast down from above the potential point to find the ground (using terrainLayer)
        // if (Physics.Raycast(potentialPoint, transform.up, 2f, terrainLayer))
        // {
        //     currentPatrolPoint = potentialPoint;
        //     haspatrolPoint = true;
        // }
    }


    private void PerformPatrol()
    {
        if (!haspatrolPoint) FindPatrolPoint();

        if (haspatrolPoint)
            navAgent.SetDestination(currentPatrolPoint);

        if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
            haspatrolPoint = false;
    
    }


    private void PerformChase()
    {
        if (playerTransform != null)
        {
            // navAgent.SetDestination(playerTransform.position);
            float step =  speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
            GameObject playerObj = GameObject.FindWithTag("Player");
            playerTransform = playerObj.transform;
            // Debug.Log(playerTransform.position);
        }
    }

   

    private void UpdateBehaviourState()
    {
        if (!isPlayerVisible && !isPlayerInRange)
        {
            PerformPatrol();
        }

        else if (isPlayerVisible && !isPlayerInRange)
        {
            PerformChase();
        }
    }

       
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Binary Bug hit the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();
        }
    }
}

// ========== Observed Player ========================================