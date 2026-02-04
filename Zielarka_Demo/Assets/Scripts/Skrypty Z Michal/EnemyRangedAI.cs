using UnityEngine;

public class EnemyRangedAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float stopDistance = 4f;

    [Header("Random Patrol")]
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float waitTimeAtPoint = 1f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float shootCooldown = 1.5f;

    private Transform player;
    private Vector2 patrolTarget;
    private bool waiting;
    private float waitTimer;
    private float lastShootTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetNewPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            EngagePlayer(distanceToPlayer);
        }
        else
        {
            RandomPatrol();
        }
    }

    private void RandomPatrol()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                waiting = false;
                SetNewPatrolPoint();
            }
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);
        FlipByMovement();

        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            waiting = true;
            waitTimer = 0f;
        }
    }

    private void SetNewPatrolPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        patrolTarget = new Vector2(transform.position.x + randomX, transform.position.y);
    }

    private void EngagePlayer(float distance)
    {
        FlipTowardsPlayer();

        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
        else
        {
            if (Time.time >= lastShootTime + shootCooldown)
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void FlipTowardsPlayer()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void FlipByMovement()
    {
        if (patrolTarget.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
}
