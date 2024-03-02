using System.Collections;
using Scripts.Game;
using UnityEngine;

namespace Scripts.Player.Gun
{
    public class Rifle : MonoBehaviour, IGun
    {
        [SerializeField]
        private int bulletDamage;

        [SerializeField]
        private bool bulletSpreadEnabled;

        [SerializeField]
        private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

        [SerializeField]
        private ParticleSystem shootingSystem;

        [SerializeField]
        private ParticleSystem impactParticleSystem;

        [SerializeField]
        private TrailRenderer bulletTrail;

        [SerializeField]
        private float shootDelay;

        [SerializeField]
        private LayerMask mask;

        private float _lastShootTime;

        public bool TryShoot()
        {
            if (_lastShootTime + shootDelay < Time.time)
            {
                shootingSystem.Play();

                Vector3 direction = GetDirection();
                Transform bulletSpawnTransform = PlayerCharacterController
                    .Instance
                    .BulletSpawnTransform;
                if (
                    Physics.Raycast(
                        bulletSpawnTransform.position,
                        direction,
                        out RaycastHit hit,
                        float.MaxValue,
                        mask
                    )
                )
                {
                    if (hit.collider != null)
                    {
                        HealthComponent hp =
                            hit.transform.gameObject.GetComponent<HealthComponent>();
                        if (hp != null)
                        {
                            DamageInfo d = new DamageInfo(bulletDamage, BulletColor.Blue);
                            hp.TakeDamage(d);
                        }
                            
                    }

                    TrailRenderer trail = Instantiate(
                        bulletTrail,
                        bulletSpawnTransform.position,
                        Quaternion.identity
                    );
                    StartCoroutine(SpawnTrail(trail, hit));
                }

                return true;
            }

            return false;
        }

        private Vector3 GetDirection()
        {
            Transform bulletSpawnTransform = PlayerCharacterController
                .Instance
                .BulletSpawnTransform;
            Vector3 direction = bulletSpawnTransform.transform.forward;
            if (bulletSpreadEnabled)
            {
                direction += new Vector3(
                    Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                    Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                    Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
                );
                direction.Normalize();
            }

            return direction;
        }

        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;

            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            trail.transform.position = hit.point;
            if (impactParticleSystem != null)
            {
                Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
            }

            Destroy(trail.gameObject, trail.time);
        }
    }
}
