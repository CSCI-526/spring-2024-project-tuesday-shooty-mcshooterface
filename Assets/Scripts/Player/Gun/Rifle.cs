using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour, IGun {
    [SerializeField] private int bulletDamage;
    [SerializeField] private bool bulletSpreadEnabled;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private float shootDelay;
    [SerializeField] private LayerMask mask;

    private float _lastShootTime;

    public bool TryShoot() {
        if (_lastShootTime + shootDelay < Time.time) {
            shootingSystem.Play();

            Vector3 direction = GetDirection();
            if (Physics.Raycast(bulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, mask)) {

                if (hit.collider != null)
                {
                    HealthComponent hp = hit.transform.gameObject.GetComponent<HealthComponent>();
                    if (hp != null) hp.TakeDamage(bulletDamage);
                }

                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

            return true;
        }

        return false;
    }

    private Vector3 GetDirection() {
        Vector3 direction = bulletSpawnPoint.transform.forward;
        if (bulletSpreadEnabled) {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z));
            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit) {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1) {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        if (impactParticleSystem != null) {
            Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        }
        
        Destroy(trail.gameObject, trail.time);
    }
}
