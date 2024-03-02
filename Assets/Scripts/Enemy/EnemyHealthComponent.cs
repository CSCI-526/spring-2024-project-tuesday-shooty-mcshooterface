using Scripts.Game;

public class EnemyHealthComponent : HealthComponent
{
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

        // TODO: Refactor
        if (
            !BulletQueueManager.Instance.DamageDealtPerEnemyType.ContainsKey(
                damage.color.ToString()
            )
        )
        {
            BulletQueueManager.Instance.DamageDealtPerEnemyType.Add(damage.color.ToString(), 0);
        }

        BulletQueueManager.Instance.DamageDealtPerEnemyType[damage.color.ToString()] += delta;

        if (_currentHealth + delta > 0 && _currentHealth <= 0)
        {
            OnDeath?.Invoke(_currentHealth);
        }
    }
}
