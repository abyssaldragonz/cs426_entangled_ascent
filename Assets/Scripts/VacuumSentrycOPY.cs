

   using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;


// ALL LINES OF CODE WITH "//" IN FRONT OF THEM ARE COMMENTED OUT FOR NOW, 
// BUT MAY BE RELEVANT TO FUTURE IMPLEMENTATION OF THE SENTRY'S ABILITIES.
// THEY ARE LEFT IN AS A REFERENCE FOR HOW THE SENTRY'S COMBAT BEHAVIOR MIGHT WORK IN THE FUTURE.

public class VacuumSentryCopy: MonoBehaviour
{

    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header ("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool haspatrolPoint;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown=1f;
    private bool isOnAttackCooldown;
    [SerializeField] private float forwardShotForce = 10f;
    [SerializeField] private float verticalShotForce = 5f;

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;

    private bool isPlayerVisible;
    private bool isPlayerInRange;

    private Rigidbody rb;
 
       private void Awake()
    {

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

        // Check if player is within engagement range
        isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
    }

   private void FireProjectile()
   {
if (projectilePrefab == null || firePoint == null) return;
        
        // Instantiate the projectile and apply forces to it
       Rigidbody projectileRb = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
       projectileRb.AddForce(transform.forward * forwardShotForce, ForceMode.Impulse);
       projectileRb.AddForce(transform.up * verticalShotForce, ForceMode.Impulse);
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

        if (haspatrolPoint)
            navAgent.SetDestination(currentPatrolPoint);

        if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
            haspatrolPoint = false;
    
    }


    private void PerformChase()
    {
        if(playerTransform != null)
            navAgent.SetDestination(playerTransform.position);
    }

    private void PerformAttack()
    {
      navAgent.SetDestination(transform.position);

       if(playerTransform != null)
       {
        transform.LookAt(playerTransform);
       }

       if(!isOnAttackCooldown)
       {
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
        //{
            PerformAttack();
        }
private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Vacuum Sentry hit the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();
        }
    }
    }



 // ========== Observed Player ========================================
  