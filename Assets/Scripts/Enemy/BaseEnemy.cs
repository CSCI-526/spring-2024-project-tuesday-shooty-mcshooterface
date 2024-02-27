using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public HealthComponent HealthComponent => _healthComponent ??= GetComponent<HealthComponent>();
    public Rigidbody RigidbodyComponent => _rigidbody ??= GetComponent<Rigidbody>();
    public CapsuleCollider ColliderComponent => _collider ??= GetComponent<CapsuleCollider>();

    public BulletColor _bulletColor;

    protected HealthComponent _healthComponent;
    protected Rigidbody _rigidbody;
    protected CapsuleCollider _collider;

    private void Start()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        _healthComponent.OnDeath += OnDeath;
    }

    private void OnDeath(int newHealth)
    {
        StartCoroutine(SelfDestruct());
    }

    protected virtual IEnumerator SelfDestruct()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.BulletQueueManager.Reload(_bulletColor);
        Destroy(gameObject);
    }
}
