using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float radius = 5f;
    [SerializeField] private IntReference blastDamage;
    [SerializeField] private float delay = 1f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float maxStartAngularVelocity = 10.0f;

    private void Start()
    {
        Vector3 randomAxis = Random.onUnitSphere;
        float randomSpeed = Random.Range(0f, maxStartAngularVelocity);
        rb.angularVelocity = randomAxis * randomSpeed;

        transform.rotation = Random.rotation;
    }
    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
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
                DamageInfo d = new DamageInfo(blastDamage.Value, BulletColor.Red, GetType().Name);
                hp.TakeDamage(d);
            }
        }
        Destroy(gameObject);
        yield return new WaitForSeconds(delay);
    }

    private void Update()
    {
        rb.AddForce(gravity * Time.deltaTime * Vector3.down, ForceMode.Acceleration);
    }
}
