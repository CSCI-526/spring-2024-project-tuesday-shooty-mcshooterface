using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    
    [SerializeField] private float radius = 5f;
    [SerializeField] private IntReference blastDamage;
    [SerializeField] private float delay = 1f;
    [SerializeField] private GameObject explosionEffect;

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Collider[] colliders = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Enemy"));
        yield return new WaitForSeconds(delay);
        Instantiate(explosionEffect, position, rotation);
        foreach (Collider c in colliders)
        {
            HealthComponent hp = c.transform.gameObject.GetComponent<HealthComponent>();
            if (hp != null)
            {
                DamageInfo d = new DamageInfo(blastDamage.Value, BulletColor.Red, GetType().Name);
                hp.TakeDamage(d);
            }
        }
        Destroy(gameObject);
    }
}
