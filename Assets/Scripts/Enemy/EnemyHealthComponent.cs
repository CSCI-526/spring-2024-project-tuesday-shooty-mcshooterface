using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : Scripts.Game.HealthComponent
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
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth + delta > 0 && _currentHealth <= 0)
        {
            OnDeath?.Invoke(_currentHealth);
        }
    }


}
