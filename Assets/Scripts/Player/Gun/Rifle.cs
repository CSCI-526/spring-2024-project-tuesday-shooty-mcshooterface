using System;
using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Player.Gun
{
    public class Rifle : MonoBehaviour, IGun
    {
        [SerializeField]
        private Transform _gunModel;

        private Animator rifleController;

        [SerializeField]
        private IntReference bulletDamage;

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
        private LayerMask mask;

        private void Start()
        {
            rifleController = _gunModel.GetComponent<Animator>();
        }

        public bool TryShoot()
        {
            shootingSystem.Play();
            rifleController.SetTrigger("onFire");

            Vector3 direction = GetDirection();
            Vector3 hitPosition = Vector3.zero;
            Vector3 hitNormal = Vector3.zero;
            Transform bulletSpawnTransform = PlayerCharacterController
                .Instance
                .BulletSpawnTransform;

            ReadOnlySpan<RaycastHit> hits = Physics.RaycastAll(
                bulletSpawnTransform.position,
                direction,
                float.MaxValue,
                mask
            );

            if (hits.Length > 0)
            {
                float closest = float.MaxValue;
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    if (hit.collider != null)
                    {
                        HealthComponent hp =
                            hit.transform.gameObject.GetComponent<HealthComponent>();
                        if (hp != null)
                        {
                            DamageInfo d =
                                new(bulletDamage.Value, BulletColor.Blue, GetType().Name);
                            hp.TakeDamage(d);
                        }
                    }

                    if (hit.distance < closest)
                    {
                        closest = hit.distance;
                        hitPosition = hit.point;
                        hitNormal = hit.normal;
                    }
                }
            }
            else
            {
                hitPosition = bulletSpawnTransform.position + direction * 100;
                hitNormal = Vector3.zero;
            }

            TrailRenderer trail = Instantiate(bulletTrail, _gunModel.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitPosition, hitNormal));

            GameManager.Instance.AudioManager.Play("RifleSFX");
            return true;
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

        private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hit, Vector3 hitNormal)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;
            WaitForSeconds wait = new WaitForSeconds(0.003f);

            while (time < 1)
            {
                Vector3 position = Vector3.Lerp(startPosition, hit, time);
                trail.AddPosition(position);
                time += 0.03f / trail.time;

                yield return wait;
            }

            //trail.transform.position = hit.point;
            trail.AddPosition(hit);
            if (impactParticleSystem != null)
            {
                Instantiate(impactParticleSystem, hit, Quaternion.LookRotation(hitNormal));
            }

            Destroy(trail.gameObject, trail.time);
        }
    }
}
