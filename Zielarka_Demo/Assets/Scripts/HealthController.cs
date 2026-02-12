using Unity.VisualScripting;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int health = 10;
    [SerializeField] private int currentHealth;
    private bool _isDamaged;
    public LayerMask whatIsEnemy;

    void Start()
    {
        currentHealth = health;
    }

    void Update()
    {
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _isDamaged =  true;
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
        print("died");
    }
}
