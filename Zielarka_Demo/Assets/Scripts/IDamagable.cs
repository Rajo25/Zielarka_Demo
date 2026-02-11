using UnityEngine;

public interface IDamagable
{
    public float Health { set; get; }
    public bool Targetable {  set; get; }
    public bool Invincible { set; get; }
    public void OnHit(int damage, Vector2 knockback);
    public void OnHit(int damage);
    public void OnDeath();
}
