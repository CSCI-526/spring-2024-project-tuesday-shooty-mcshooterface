using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : Scripts.Game.HealthComponent
{
    [SerializeField] int _maxHealth;

    DamageVFX damageVFX;
    protected void Start()
    {
        _currentHealth = _maxHealth;
        damageVFX = FindObjectOfType<DamageVFX>();
    }

    public override void TakeDamage(DamageInfo damage)
    {
        base.TakeDamage(damage);
        damageVFX.DamageFlash();
    }
}

