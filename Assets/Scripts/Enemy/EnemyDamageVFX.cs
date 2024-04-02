using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem _damageParticles;

    public void Play()
    {
        _damageParticles.Play();
    }

    public void DeathPlay()
    {
        Play();
        transform.parent = null;
        StartCoroutine(WaitUntilDeath(_damageParticles.main.duration));
    }

    private IEnumerator WaitUntilDeath(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
