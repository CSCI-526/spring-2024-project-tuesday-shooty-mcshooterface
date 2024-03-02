using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] EnemyType _enemyType;
    [SerializeField] protected EnemyStatObject _enemyStatTunable;
    [SerializeField] protected SuperEffectiveObject _superEffectiveTunable;

    protected EnemyHealthComponent _healthComponent;
    protected Rigidbody _rigidbody;
    protected CapsuleCollider _collider;

    public EnemyType EnemyType => _enemyType;
    public EnemyHealthComponent HealthComponent => _healthComponent;
    public Rigidbody RigidbodyComponent => _rigidbody;
    public CapsuleCollider ColliderComponent => _collider ??= GetComponent<CapsuleCollider>();
    public EnemyStatObject EnemyStatTunable => _enemyStatTunable;
    public SuperEffectiveObject SuperEffectiveTunable => _superEffectiveTunable;

    protected virtual void Start()
    {
        _healthComponent = GetComponent<EnemyHealthComponent>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        _healthComponent.Construct(this);
        _healthComponent.OnDeath += OnDeath;
    }

    private void OnDeath(int newHealth)
    {
        StartCoroutine(SelfDestruct());
    }

    protected virtual IEnumerator SelfDestruct()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.BulletQueueManager.Reload(_enemyStatTunable.GetDropColor(_enemyType));
        Destroy(gameObject);
    }
}

public enum EnemyType
{
    Ogre,
    Swarm,
    Flying
}
