using UnityEngine;

namespace Player.Combat
{
    public class HealthController : MonoBehaviour
    {
        public int maxHealth = 100;
        private int _currentHealth;

        void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= (int)damage;

            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            print("Player Died");
        }
    }
}