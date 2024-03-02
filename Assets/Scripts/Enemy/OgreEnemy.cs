using System;
using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;

public class OgreEnemy : BaseEnemy
{
    [SerializeField] private GameObjectCollection enemyCollection;

    [Header("Ogre Enemy Vars")]
    [SerializeField]
    private float _knockbackForce = 100f;
    [SerializeField]
    private float _knockbackStunDuration = 1.5f;

    private bool _isStunned = false;

    protected override void Start() {
        base.Start();
        enemyCollection.Add(gameObject);
    }

    private void OnDestroy() {
        enemyCollection.Remove(gameObject);
    }

    void Update() {
        var player = PlayerCharacterController.Instance;
        if (player == null)
        {
            return;
        }

        Vector3 toVector = player.transform.position - transform.position;

        if (!_isStunned)
        {
            if (toVector.sqrMagnitude > 1 + _collider.radius * 2)
            {
                RigidbodyComponent.velocity = toVector.normalized * _enemyStatTunable.OgreSpeed;
            }
            else
            {
                Attack(player.gameObject);
            }
        }
    }

    private void Attack(GameObject entity)
    {
        Vector3 toVector = entity.transform.position - transform.position;

        DamageInfo d = new DamageInfo(1);
        entity.GetComponent<HealthComponent>().TakeDamage(d);
        
        RigidbodyComponent.velocity = Vector3.zero;
        RigidbodyComponent.AddForce((-toVector.normalized + Vector3.up).normalized * _knockbackForce);
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        _isStunned = true;
        yield return new WaitForSeconds(_knockbackStunDuration);
        _isStunned = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        // TODO:Contact
    }
}
