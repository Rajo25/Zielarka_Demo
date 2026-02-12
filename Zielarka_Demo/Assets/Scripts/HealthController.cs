using Unity.VisualScripting;
using UnityEngine;

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

    void Start()
    {
        currentHealth = health;
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
    }

    void TakeDamage(int damage)
    {
        if (_isInvincible) return;
        
        currentHealth -= damage;
        _isDamaged =  true;
        
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
        }
    }
    void Die()
    {
        _isDead = true;
        print("died");
    }
    
}
