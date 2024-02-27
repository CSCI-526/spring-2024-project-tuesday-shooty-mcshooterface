using System.Collections;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;

public class OgreEnemy : BaseEnemy
{
    [Header("Ogre Enemy Vars")]
    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _knockbackForce = 100f;

    private float _knockbackStunDuration = 1.5f;

    private bool _isStunned = false;

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
                RigidbodyComponent.velocity = toVector.normalized * _speed;
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
        entity.GetComponent<HealthComponent>().TakeDamage(1);
        RigidbodyComponent.velocity = Vector3.zero;
        RigidbodyComponent.AddForce((-toVector.normalized + Vector3.up).normalized * _knockbackForce);
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        _isStunned = true;
        yield return new WaitForSeconds(1.5f);
        _isStunned = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        Debug.Log("Contact");
    }
}
