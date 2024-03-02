using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : Scripts.Game.HealthComponent
{
    [SerializeField] int _maxHealth;

    protected void Start()
    {
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(DamageInfo damage)
    {
        base.TakeDamage(damage);

        // TODO: Refactor
        if (BulletQueueManager.Instance.DamageDealtPerEnemyType.ContainsKey(damage.source) == false)
        {
            BulletQueueManager.Instance.DamageDealtPerEnemyType.Add(damage.source, 0);
        }

        BulletQueueManager.Instance.DamageDealtPerEnemyType[damage.source] += damage.damage;
    }
}

