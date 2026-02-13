using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Header("hpControl")]
    public int health = 10;
    [SerializeField] private int currentHealth;
    private bool _isDamaged;
    public LayerMask whatIsEnemy;

    [Header("parry")] 
    private bool _isInvincible;
    public float invincibilityWindow;
    private float _invincibilityTimer;
    
    [Header("death")]
    public bool isPlayer;
    private bool _isDead;
    public float dyingTime;
    private float _dyingTimer;

    public bool is_hurt;

    public Animator anim;
    public Animator enemyAnim;

    void Start()
    {
        currentHealth = health;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (_isInvincible)
        {
            _invincibilityTimer -= Time.deltaTime;

            if (_invincibilityTimer <= 0)
            {
                _isInvincible = false;
            }
        }
        

        if (currentHealth <= 0 && !_isDead)
        {
            Die();

        }

        if (_isDead && isPlayer)
        {
            _dyingTimer -= Time.deltaTime;

            if (_dyingTimer <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        
        //anim.SetBool("is_hurt", false);
        //anim.SetBool("is_dead", false);

    }

    void TakeDamage(int damage)
    {
        if (_isInvincible) return;
        
        currentHealth -= damage;
        _isDamaged = true;
        anim.SetTrigger("is_Hurt");
        enemyAnim.SetTrigger("is_Hurt");



        _isInvincible = true;
        _invincibilityTimer = invincibilityWindow;
    }

    void OnCollisionEnter2D(Collision2D whatIsEnemy)
    {
        AttackController damage = whatIsEnemy.gameObject.GetComponent<AttackController>();
        if (damage != null)
        { 
            TakeDamage(damage.attackDamage);
            _isDamaged = false;
            anim.SetTrigger("is_Hurt");
        }
    }
    void Die()
    {
        _isDead = true;
        _dyingTimer = dyingTime;
        
        if (isPlayer)
        {
            anim.SetTrigger("is_dead");
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
    
}
