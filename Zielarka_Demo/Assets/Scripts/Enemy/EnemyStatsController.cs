using Player.Combat;
using UnityEditor;
using UnityEngine;

public class EnemyStatsController : MonoBehaviour
{
    public int maxHealth = 10;
    private int _currentHealth;
    public int maxStamina = 10;
    private int _currentStamina;
    public float damage = 1;
    public float knockbackForce = 0.5f;
    void Start()
    {
       _currentHealth = maxHealth;
       _currentStamina = maxStamina;
    }
    
    
    private void Die()
    {
        print("Enemy Died");
    }
}
