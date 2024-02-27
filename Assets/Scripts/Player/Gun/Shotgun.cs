using System.Collections;
using Scripts.Game;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    [Header("ShotGun Var")]
    [SerializeField] int numPellets;
    [SerializeField] float spreadAngle;
    [SerializeField] int bulletDamage;
    [SerializeField] float firingRate = 1;

    [Header("References")]
    [SerializeField] LayerMask mask;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] TrailRenderer bulletTrail;

    private float elapsed = 0;

    public bool TryShoot()
    {
        if (elapsed <= 0)
        {
            elapsed = 1.0f / firingRate;
            FirePellets();
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (elapsed > 0) elapsed -= Time.deltaTime;
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
            point = bulletSpawnPoint.transform.TransformPoint(point);
            Fire((point - bulletSpawnPoint.transform.position).normalized);
        }
    }

    private void Fire(Vector3 direction)
    {
        Physics.Raycast(bulletSpawnPoint.transform.position, direction, out RaycastHit hit, float.MaxValue, mask);
        if (hit.collider != null) 
        {
            HealthComponent hp = hit.transform.gameObject.GetComponent<HealthComponent>();
            if (hp != null) hp.TakeDamage(bulletDamage);
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
