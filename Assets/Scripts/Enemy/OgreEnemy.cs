using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class OgreEnemy : BaseEnemy
{
    [Header("Ogre Enemy Vars")]
    [SerializeField]
    private float _speed = 1f;

    void Update()
    {
        GameObject player = GameManager.Instance.PlayerReference;
        if (player == null)
        {
            return;
        }

        Vector3 toVector = player.transform.position - transform.position;
        if (toVector.sqrMagnitude > 1 + _collider.radius * 2)
        {
            RigidbodyComponent.velocity = toVector.normalized * _speed;
        }
        else
        {
            RigidbodyComponent.velocity = Vector3.zero;
        }
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
