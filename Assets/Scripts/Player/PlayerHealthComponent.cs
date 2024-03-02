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
}

