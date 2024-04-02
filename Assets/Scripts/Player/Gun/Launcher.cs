using Scripts.Player;
using UnityEngine;

public class Launcher : MonoBehaviour, IGun
{
    private const float MAX_RANGE = 30f;
    [SerializeField]
    private GameObject _grenadeModel;

    [SerializeField]
    private GameObject grenade;

    [SerializeField]
    private float range = 30f;

    public bool TryShoot()
    {
        Transform gunTransform = _grenadeModel.transform;
        Transform bulletSpawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;
        RaycastHit[] hits = Physics.RaycastAll(
            bulletSpawnTransform.position,
            bulletSpawnTransform.forward,
            MAX_RANGE,
            LayerMask.GetMask(LayerMask.LayerToName(2))
        );

        Vector3 direction = GunHelper.GetDirection(hits, gunTransform, MAX_RANGE);

        GameObject grenadeInstance = Instantiate(
            grenade,
            gunTransform.position + direction * 2,
            bulletSpawnTransform.rotation
        );
        grenadeInstance
            .GetComponent<Rigidbody>()
            .AddForce(direction * range, ForceMode.Impulse);
        Scripts.Game.GameManager.Instance.AudioManager.Play("GrenadeLauncherSFX");
        return true;
    }
}
