using UnityEngine;

namespace Player.Combat
{
    public class PlayerStatsController : MonoBehaviour
    {
        public int maxHealth = 100;
        private int _currentHealth;
        public int maxMana = 10;
        private int _currentMana;
        

        void Start()
        {
            _currentHealth = maxHealth;
            _currentMana = maxMana;
        }
        

        private void Die()
        {
            print("Player Died");
        }
    }
}