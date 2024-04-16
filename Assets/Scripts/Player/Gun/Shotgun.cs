using System.Collections;
using ScriptableObjectArchitecture;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    [Header("ShotGun Var")]
    [SerializeField]
    private GameObject _shotgunModel;

    [SerializeField]
    int numPellets;

    [SerializeField]
    float spreadAngle;

    [SerializeField]
    private IntReference bulletDamage;

    [SerializeField]
    private Animator shotgunController;

    [Header("References")]
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    Transform bulletSpawnTransform;

    [SerializeField]
    TrailRenderer bulletTrail;

    private void Start()
    {
        //shotgunController = _shotgunModel.GetComponent<Animator>();
    }

    public bool TryShoot()
    {
        shotgunController.SetTrigger("onFire");
        FirePellets();
        Scripts.Game.GameManager.Instance.AudioManager.Play("ShotgunSFX");
        return true;
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
        Vector3 hitPoint;
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
                DamageInfo d = new DamageInfo(
                    bulletDamage.Value,
                    BulletColor.Green,
                    GetType().Name
                );
                hp.TakeDamage(d);
            }

            hitPoint = hit.point;
        }
        else
        {
            hitPoint = bulletSpawnTransform.transform.position + direction * 100;
        }

        TrailRenderer trail = Instantiate(
            bulletTrail,
            _shotgunModel.transform.position,
            Quaternion.identity
        );
        StartCoroutine(SpawnTrail(trail, hitPoint));
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            Vector3 position = Vector3.Lerp(startPosition, hit, time);
            trail.AddPosition(position);
            time += Time.deltaTime / trail.time;

            yield return 0;
        }

        trail.AddPosition(hit);

        Destroy(trail.gameObject, trail.time);
    }
}
