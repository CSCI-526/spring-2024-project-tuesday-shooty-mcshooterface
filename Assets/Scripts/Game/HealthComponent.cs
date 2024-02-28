using UnityEngine;

namespace Scripts.Game
{
    public class HealthComponent : MonoBehaviour
    {
        public delegate void HealthEvent(int newHealth);

        public HealthEvent OnHealthChanged;
        public HealthEvent OnDeath;

        public int CurrentHealth => _currentHealth;

        protected int _currentHealth;

        public virtual void TakeDamage(DamageInfo damage)
        {
            _currentHealth -= damage.damage;
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth + damage.damage > 0 && _currentHealth <= 0)
            {
                OnDeath?.Invoke(_currentHealth);
            }
        }
    }

    public class DamageInfo
    {
        public int damage;
        public BulletColor color = BulletColor.Empty;
    
        public DamageInfo(int damage) { this.damage = damage; }
        public DamageInfo(int damage, BulletColor color) { this.damage = damage; this.color = color; }
    }
}