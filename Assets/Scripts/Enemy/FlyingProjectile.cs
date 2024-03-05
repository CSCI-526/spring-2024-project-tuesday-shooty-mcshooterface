using Scripts.Game;
using UnityEngine;

[Enemy]
public class FlyingProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && (other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Hit by Flying Projectile!");
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
