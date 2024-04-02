using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;

[Enemy]
public class SwarmProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != null && (other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Swarm Projectile Hit!");
            HealthComponent hp = other.gameObject.GetComponent<HealthComponent>();
            if (hp != null)
            {
                DamageInfo d = new DamageInfo(1, BulletColor.Empty, GetType().Name);
                hp.TakeDamage(d);
                Destroy(gameObject);
            }
        }

    }
}
