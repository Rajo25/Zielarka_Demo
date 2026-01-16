using UnityEngine;
using AbilitiesSystem.Scripts;   

public class EnemyMeleeAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Random Patrol")]
    [SerializeField] private float patrolRadius = 4f;
    [SerializeField] private float waitTimeAtPoint = 1f;

    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private Transform player;
    private Vector2 patrolTarget;
    private bool waiting;
    private float waitTimer;
    private float lastAttackTime;

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
            ChaseAndAttack(distanceToPlayer);
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
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        patrolTarget = (Vector2)transform.position + randomPoint;
    }

    private void ChaseAndAttack(float distance)
    {
        FlipTowardsPlayer();

        if (distance > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
        else
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        HealthController health = player.GetComponent<HealthController>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
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
