using UnityEngine;
using AbilitiesSystem.Scripts; 

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;      
    [SerializeField] private float lifeTime = 3f;  

    private void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthController health = collision.GetComponent<HealthController>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject); 
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject); 
        }
    }
}
