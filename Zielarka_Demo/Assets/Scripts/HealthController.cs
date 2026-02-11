using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int health = 10;
    [SerializeField] private int currentHealth;

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

    void Die()
    {
        print("died");
    }
}
