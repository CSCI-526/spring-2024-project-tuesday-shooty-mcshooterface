using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    [Header("ShotGun Var")]
    [SerializeField]
    int numPellets;

    [SerializeField]
    float spreadAngle;

    [SerializeField] private IntReference bulletDamage;

    [SerializeField]
    float firingRate = 1;

    [Header("References")]
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    Transform bulletSpawnTransform;

    [SerializeField]
    TrailRenderer bulletTrail;

    private float elapsed = 0;

    public bool TryShoot()
    {
        if (elapsed <= 0)
        {
            elapsed = 1.0f / firingRate;
            FirePellets();
            Scripts.Game.GameManager.Instance.AudioManager.Play("ShotgunSFX");
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (elapsed > 0)
            elapsed -= Time.deltaTime;
    }

    private void FirePellets()
    {
        //project a circle in front of you, choose a random point on a circle to calculate trajectory
        float R = Mathf.Tan(spreadAngle / 180.0f * Mathf.PI);
        for (int i = 0; i < numPellets; i++)
        {
            float r = R * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
            float angle = Random.Range(0.0f, 1.0f) * 2 * Mathf.PI;

            Vector3 point = new Vector3(r * Mathf.Cos(angle), r * Mathf.Sin(angle), 1.0f);
            Transform bulletSpawnTransform = PlayerCharacterController
                .Instance
                .BulletSpawnTransform;
            point = bulletSpawnTransform.transform.TransformPoint(point);
            Fire((point - bulletSpawnTransform.transform.position).normalized);
        }
    }

    private void Fire(Vector3 direction)
    {
        Transform bulletSpawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;
        Physics.Raycast(
            bulletSpawnTransform.transform.position,
            direction,
            out RaycastHit hit,
            float.MaxValue,
            mask
        );
        if (hit.collider != null)
        {
            HealthComponent hp = hit.transform.gameObject.GetComponent<HealthComponent>();
            if (hp != null)
            {
                DamageInfo d = new DamageInfo(bulletDamage.Value, BulletColor.Green, GetType().Name);
                hp.TakeDamage(d);
            }
                
        }
        TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, hit));
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

        Destroy(trail.gameObject, trail.time);
    }
}
