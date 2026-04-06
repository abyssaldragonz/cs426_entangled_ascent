using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBunny : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float stoppingDistance = 2f;
    public float visionRange = 10f;
    public float wanderRadius = 5f;      // How far it wanders
    public float wanderInterval = 3f;    // Time between changing direction

    [Header("Shooting Settings")]
    public GameObject ballPrefab;
    public Transform firePoint;
    public float shootRange = 10f;
    public float fireRate = 1f;

    private Transform player;
    private Rigidbody rb;
    private float fireCooldown = 0f;
    private Vector3 wanderTarget;
    private float wanderTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        ChooseNewWanderTarget();
    }

    void FixedUpdate()
    {
        if (player != null && CanSeePlayer())
        {
            ChaseAndShoot();
        }
        else
        {
            Wander();
        }

        if (fireCooldown > 0f)
            fireCooldown -= Time.fixedDeltaTime;
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (player.position - origin);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, visionRange))
        {
            // Debug.Log("Dust Bunny sees player!");
            return hit.transform.CompareTag("Player");
        }
        return false;
    }

    void ChaseAndShoot()
    {
        Vector3 direction = (player.position - transform.position);
        float distance = direction.magnitude;

        // Move towards player
        if (distance > stoppingDistance)
        {
            float move = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, move);
        }

        // Face the player
        transform.LookAt(player.position);

        // Shoot
        if (distance <= shootRange && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void Wander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        if (wanderTimer <= 0f || Vector3.Distance(transform.position, wanderTarget) < 0.5f)
        {
            ChooseNewWanderTarget();
        }

        float move = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, move);

        // Slowly rotate towards wander target
        Vector3 lookPos = new Vector3(wanderTarget.x, transform.position.y, wanderTarget.z);
        transform.LookAt(lookPos);
    }

    void ChooseNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
        wanderTimer = wanderInterval;
    }

    void Shoot()
    {
        if (ballPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Ball Prefab or FirePoint not assigned!");
            return;
        }

        firePoint.LookAt(player);

        Rigidbody projectileRb = Instantiate(ballPrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        projectileRb.linearVelocity += Vector3.up * 3;
        projectileRb.AddForce(projectileRb.gameObject.transform.forward * 5f * Time.deltaTime);

        // destroy after 3 seconds
        Destroy(projectileRb.gameObject, 3f);
    }

    // ========== Collided with Player ===================================
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Dust Bunny caught the player!");
            other.gameObject.GetComponent<PlayerMovement>().LoseLife();
        }
    }
}