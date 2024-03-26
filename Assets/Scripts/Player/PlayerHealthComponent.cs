using Scripts.Game;
using UnityEngine;

public class PlayerHealthComponent : Scripts.Game.HealthComponent {
    public static PlayerHealthComponent Instance;
    
    [SerializeField] int _maxHealth;

    DamageVFX damageVFX;

    private void Awake() {
        Instance = this;
        _currentHealth = _maxHealth;
    }

    protected void Start()
    {
        damageVFX = FindObjectOfType<DamageVFX>();
    }

    public override void TakeDamage(DamageInfo damage)
    {
        // TODO: Refactor
        if (BulletQueueManager.Instance.DamageDealtPerEnemyType.ContainsKey(damage.source) == false)
        {
            Debug.LogError($"Damage source: {damage.source} doesn't have a [Enemy] tag...");
        }
        else
        {
            BulletQueueManager.Instance.DamageDealtPerEnemyType[damage.source] += damage.damage;
        }

        base.TakeDamage(damage);
        damageVFX.DamageFlash();

    }
}

