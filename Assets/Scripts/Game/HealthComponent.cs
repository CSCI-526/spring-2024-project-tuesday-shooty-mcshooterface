using UnityEngine;

namespace Scripts.Game
{
    public class HealthComponent : MonoBehaviour
    {
        public delegate void HealthEvent<T>(in T args);

        public HealthEvent<DamageInfo> OnDeath;
        public HealthEvent<(DamageInfo damage, int newHealth)> OnDamageTaken;
        public HealthEvent<(int healing, int newHealth)> OnHealed;

        public int CurrentHealth => _currentHealth;
        public virtual int MaxHealth { get => 0; }

        protected int _currentHealth;

        public virtual void TakeDamage(DamageInfo damage)
        {
            _currentHealth -= damage.damage;
            OnDamageTaken?.Invoke((damage, _currentHealth));

            if (_currentHealth + damage.damage > 0 && _currentHealth <= 0)
            {
                OnDeath?.Invoke(damage);
            }
        }

        public virtual void HealHealth(int healing)
        {
            _currentHealth += healing;
            OnHealed?.Invoke((healing, _currentHealth));
        }

    }

    public class DamageInfo
    {
        public int damage;
        public BulletColor color = BulletColor.Empty;
        public string source;

        public DamageInfo(int damage, BulletColor color, string source)
        {
            this.damage = damage;
            this.color = color;
            this.source = source;
        }
    }
}
