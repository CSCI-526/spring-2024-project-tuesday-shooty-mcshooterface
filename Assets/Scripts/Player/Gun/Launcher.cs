using Scripts.Player;
using UnityEngine;

public class Launcher : MonoBehaviour, IGun
{
    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private GameObject grenade;

    [SerializeField]
    private float range = 15f;

    private float _lastShootTime;

    public bool TryShoot()
    {
        if (_lastShootTime + shootDelay >= Time.time)
            return false;
        Transform bulletSpawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;

        GameObject grenadeInstance = Instantiate(
            grenade,
            bulletSpawnTransform.position + bulletSpawnTransform.forward * 2,
            bulletSpawnTransform.rotation
        );
        grenadeInstance
            .GetComponent<Rigidbody>()
            .AddForce(bulletSpawnTransform.forward * range, ForceMode.Impulse);
        return true;
    }
}
