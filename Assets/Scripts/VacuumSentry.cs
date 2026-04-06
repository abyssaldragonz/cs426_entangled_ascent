using UnityEngine;
using System.Collections;

public class VacuumSentry : MonoBehaviour
{
    private enum SentryState
    {
        Idle,
        Attack,
        Cooldown
    }

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Detection")]
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 12f;

    private SentryState currentState = SentryState.Idle;
    private bool playerDetected = false;
    private bool isCoolingDown = false;

    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        DetectPlayer();
        UpdateState();
    }

    private void DetectPlayer()
    {
        playerDetected = Physics.CheckSphere(
            transform.position,
            visionRange,
            playerLayerMask
        );
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case SentryState.Idle:
                if (playerDetected && !isCoolingDown)
                {
                    currentState = SentryState.Attack;
                }
                break;

            case SentryState.Attack:
                AttackPlayer();
                currentState = SentryState.Cooldown;
                StartCoroutine(CooldownRoutine());
                break;

            case SentryState.Cooldown:
                if (!playerDetected && !isCoolingDown)
                {
                    currentState = SentryState.Idle;
                }
                else if (playerDetected && !isCoolingDown)
                {
                    currentState = SentryState.Attack;
                }
                break;
        }
    }

    private void AttackPlayer()
    {
        if (projectilePrefab == null || firePoint == null || playerTransform == null)
            return;

        Vector3 lookTarget = new Vector3(
            playerTransform.position.x,
            transform.position.y,
            playerTransform.position.z
        );
        transform.LookAt(lookTarget);

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (playerTransform.position - firePoint.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(attackCooldown);
        isCoolingDown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}