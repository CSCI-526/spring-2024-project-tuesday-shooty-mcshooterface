using Scripts.Player;
using UnityEngine;

public class Launcher : MonoBehaviour, IGun
{
    [SerializeField]
    private GameObject grenade;

    [SerializeField]
    private float range = 30f;

    public bool TryShoot()
    {
        Transform bulletSpawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;

        GameObject grenadeInstance = Instantiate(
            grenade,
            bulletSpawnTransform.position + bulletSpawnTransform.forward * 2,
            bulletSpawnTransform.rotation
        );
        grenadeInstance
            .GetComponent<Rigidbody>()
            .AddForce(bulletSpawnTransform.forward * range, ForceMode.Impulse);
        Scripts.Game.GameManager.Instance.AudioManager.Play("GrenadeLauncherSFX");
        return true;
    }
}
