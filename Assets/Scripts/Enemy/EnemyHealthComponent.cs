using Scripts.Game;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    [SerializeField] EnemyDamageVFX _damageVFX;
    private BaseEnemy _enemy;

    public void Construct(BaseEnemy enemy)
    {
        _enemy = enemy;

        _currentHealth = _enemy.EnemyStatTunable.GetHealth(_enemy.EnemyType);
    }

    public override void TakeDamage(DamageInfo damage)
    {
        int delta = (int)(
            damage.damage
            * _enemy.SuperEffectiveTunable.GetMultiplier(_enemy.EnemyType, damage.color)
        );

        _currentHealth -= delta;
        OnDamageTaken?.Invoke((damage, _currentHealth));

        if (_currentHealth + delta > 0 && _currentHealth <= 0)
        {
            OnDeath?.Invoke(damage);
            _damageVFX.DeathPlay();
        }
        else
        {
            _damageVFX?.Play();
        }

        string damageSource = damage.color.ToString();
        Debug.Log($"Enemy took {delta} damage from {damageSource} color bullet");
        if (!BulletQueueManager.Instance.AmmoDamageDealt.ContainsKey(damageSource))
        {
            Debug.LogError($"Damage source: {damageSource} is not defined in AmmoDamageDealt");
            return;
        }

        BulletQueueManager.Instance.AmmoDamageDealt[damageSource] += delta;
    }

    public override int MaxHealth { get => _enemy.EnemyStatTunable.GetHealth(_enemy.EnemyType); }
}
