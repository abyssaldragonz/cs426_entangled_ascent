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
    [SerializeField] private float visionRange = 30f;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 12f;

    private SentryState currentState = SentryState.Idle;
    private bool playerDetected = false;
    private bool isCoolingDown = false;

    public AudioSource audioSource;
    public AudioClip attackClip;

    private void Update()
    {
        DetectPlayer();
        UpdateState();
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, visionRange, playerLayerMask);

        if (hits.Length > 0)
        {
            playerDetected = true;

            // Use root in case the player's collider is on a child object
            playerTransform = hits[0].transform.root;
        }
        else
        {
            playerDetected = false;
            playerTransform = null;
        }
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
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
        }

        if (projectilePrefab == null || firePoint == null || playerTransform == null)
            return;

        // Aim at the player's actual position
        Vector3 targetPoint = playerTransform.position;

        // Rotate the sentry body only left/right
        Vector3 bodyDirection = targetPoint - transform.position;
        bodyDirection.y = 0f;

        if (bodyDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(bodyDirection);
        }

        // Rotate fire point fully toward player, including up/down
        Vector3 shotDirection = (targetPoint - firePoint.position).normalized;
        if (shotDirection != Vector3.zero)
        {
            firePoint.rotation = Quaternion.LookRotation(shotDirection);
        }

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody projRB = projectile.GetComponent<Rigidbody>();
        if (projRB != null)
        {
            projRB.linearVelocity = shotDirection * projectileSpeed;
        }

        Destroy(projectile, 3f);
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