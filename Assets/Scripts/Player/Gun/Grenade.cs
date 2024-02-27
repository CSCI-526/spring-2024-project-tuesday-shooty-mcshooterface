using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    
    [SerializeField] private float radius = 10f;
    [SerializeField] private int blastDamage = 10;
    [SerializeField] private float delay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", delay);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        transform.localScale = new Vector3(radius, radius, radius);
        foreach (Collider c in colliders)
        {
            HealthComponent hp = c.transform.gameObject.GetComponent<HealthComponent>();
            if (hp != null) hp.TakeDamage(blastDamage);
        }
        Destroy(gameObject);
    }
}
