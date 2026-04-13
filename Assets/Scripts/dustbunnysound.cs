using UnityEngine;

public class DustBunnysound : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float stoppingDistance = 2f;
    public float visionRange = 10f;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    [Header("Shooting Settings")]
    public GameObject ballPrefab;
    public Transform firePoint;
    public float shootRange = 10f;
    public float fireRate = 1f;

    [Header("Audio")]
    public AudioSource footstepAudio;
    public AudioSource shootAudio;

    private Transform player;
    private Rigidbody rb;
    private float fireCooldown;
    private Vector3 wanderTarget;
    private float wanderTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        ChooseNewWanderTarget();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (CanSeePlayer())
            ChaseAndShoot();
        else
            Wander();

        if (fireCooldown > 0f)
            fireCooldown -= Time.fixedDeltaTime;
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (player.position - origin).normalized;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, visionRange))
        {
            return hit.transform.CompareTag("Player");
        }
        return false;
    }

    void ChaseAndShoot()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            Vector3 newPos = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.fixedDeltaTime
            );

            rb.MovePosition(newPos);
        }

        transform.LookAt(player.position);

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

        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            wanderTarget,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        Vector3 lookPos = new Vector3(wanderTarget.x, transform.position.y, wanderTarget.z);
        transform.LookAt(lookPos);
    }

    void ChooseNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;

        wanderTarget = new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y,
            transform.position.z + randomCircle.y
        );

        wanderTimer = wanderInterval;
    }

    void Shoot()
    {
        if (ballPrefab == null || firePoint == null) return;

        firePoint.LookAt(player);

        GameObject bullet = Instantiate(ballPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();

        if (rbBullet != null)
        {
            rbBullet.linearVelocity = Vector3.zero;
            rbBullet.AddForce(firePoint.forward * 10f, ForceMode.Impulse);
        }
 if (shootAudio != null)
    {
        shootAudio.pitch = Random.Range(0.95f, 1.05f);
        shootAudio.Play();
    }
        Destroy(bullet, 3f);
    }

    // 🔊 Animation Event Function
    public void PlayFootstep()
    {
        if (footstepAudio == null) return;

        footstepAudio.pitch = Random.Range(0.9f, 1.1f);
        footstepAudio.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Dust Bunny caught the player!");

            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            if (pm != null)
                pm.LoseLife();
        }
    }
}