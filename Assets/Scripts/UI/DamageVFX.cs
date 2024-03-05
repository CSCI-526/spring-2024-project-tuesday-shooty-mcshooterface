using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
public class DamageVFX : MonoBehaviour
{
    [SerializeField] Animator damageFlash;
    [SerializeField] CinemachineImpulseSource screenShake;

    public void DamageFlash()
    {
        damageFlash.SetTrigger("Flash");
        screenShake.GenerateImpulse();
    }
}
