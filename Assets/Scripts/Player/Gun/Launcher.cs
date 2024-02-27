using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour, IGun
{
    [SerializeField] private Transform grenadeSpawnPoint;
    [SerializeField] private float shootDelay;
    [SerializeField] private GameObject grenade;
    [SerializeField] private float range = 15f;
    
    private float _lastShootTime;

    public bool TryShoot()
    {
        if (_lastShootTime + shootDelay >= Time.time) return false;
        
        GameObject grenadeInstance = Instantiate(grenade, grenadeSpawnPoint.position + grenadeSpawnPoint.forward * 2, grenadeSpawnPoint.rotation);
        grenadeInstance.GetComponent<Rigidbody>().AddForce(grenadeSpawnPoint.forward * range, ForceMode.Impulse);
        return true;
    }
}
