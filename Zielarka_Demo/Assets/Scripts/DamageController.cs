using UnityEngine;

public class DamageController : MonoBehaviour, IDamageable
{
    public bool canTurnInvincible = false;
    public float invincibleTime = 0.5f;

    [SerializeField]private float _health;
    private bool _targetable = true;
    private bool _invincible = false;
    [SerializeField] private bool _isAlive = true;

    private Rigidbody2D _rb;
    private Collider2D _physicsCollider;

    public float Health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health <= 0)
            {
                _isAlive = false;
            }
        }
    }

    public bool Targetable
    {
        get => _targetable;
        set => _targetable = value;
    }

    public bool Invincible
    {
        get => _invincible;
        set => _invincible = value;
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if (!Invincible)
        {
            Health -= damage;
            _rb.AddForce(knockback, ForceMode2D.Impulse);
            print("dealt Damage");

            if (canTurnInvincible)
            {
                Invincible = true;
            }
        }
    }

    public void OnHit(float damage)
    {
        OnHit(damage, Vector2.zero);
    }
}