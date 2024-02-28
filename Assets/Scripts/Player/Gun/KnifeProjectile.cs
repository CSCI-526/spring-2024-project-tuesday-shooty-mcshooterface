using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using Scripts.Player.Gun;

public class KnifeProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private int _damage = 5;
    void Start()
    {
        // Debug
        GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other != null) 
        {
            HealthComponent hp = other.gameObject.GetComponent<HealthComponent>();
            if (hp != null) 
            {
                DamageInfo d = new DamageInfo(_damage);
                hp.TakeDamage(d);
                Destroy(gameObject);
            }
        }

    }
}
