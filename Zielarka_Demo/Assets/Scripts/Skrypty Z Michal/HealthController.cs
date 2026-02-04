using UnityEngine;

namespace AbilitiesSystem.Scripts
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int currentHealth;

        public void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;

            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}