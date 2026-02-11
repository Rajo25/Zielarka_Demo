using UnityEngine;

public class EnemyDamageInput : MonoBehaviour
{
    public float damage;
    public float knockbackForce;
    
    
    void onCollisionEnter(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;
            damageable.OnHit(damage, knockback);
        }
    }
}
