using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    
    [SerializeField] private float radius = 5f;
    [SerializeField] private int blastDamage = 10;
    [SerializeField] private float delay = 1f;
    [SerializeField] private GameObject explosionEffect;

    private void OnCollisionEnter(Collision other)
    {
        Explode();
    }

    private void Explode()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Collider[] colliders = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Enemy"));
        Instantiate(explosionEffect, position, rotation);
        foreach (Collider c in colliders)
        {
            HealthComponent hp = c.transform.gameObject.GetComponent<HealthComponent>();
            if (hp != null)
            {
                DamageInfo d = new DamageInfo(blastDamage, BulletColor.Red);
                hp.TakeDamage(d);
            }
        }
        Destroy(gameObject);
    }
}
