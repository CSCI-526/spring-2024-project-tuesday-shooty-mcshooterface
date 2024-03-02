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
        int delta = (int) (damage.damage * _enemy.SuperEffectiveTunable.GetMultiplier(_enemy.EnemyType, damage.color));

        _currentHealth -= delta;
        OnDamageTaken?.Invoke((damage, _currentHealth));

        if (_currentHealth + delta > 0 && _currentHealth <= 0)
        {
            OnDeath?.Invoke(_currentHealth);
        }
    }


}
