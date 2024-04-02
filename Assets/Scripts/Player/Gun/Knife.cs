using Scripts.Game;
using Scripts.Player;
using UnityEngine;

public class Knife : MonoBehaviour, IGun
{
    public const float MAX_RANGE = 30f;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _knifeModel;
    public GameObject KnifePrefab;
    public float KnifeSpeed = 30;

    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void ThrowKnife()
    {
        Transform spawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;
        RaycastHit[] hits = Physics.RaycastAll(
            spawnTransform.position,
            spawnTransform.forward,
            MAX_RANGE,
            LayerMask.GetMask(LayerMask.LayerToName(2))
        );

        Vector3 direction = GunHelper.GetDirection(hits, _knifeModel.transform, MAX_RANGE);
        GameObject kf =
            Instantiate(KnifePrefab, _knifeModel.transform.position, spawnTransform.rotation)
            as GameObject;
        kf.GetComponent<Rigidbody>().velocity = direction * KnifeSpeed;
        GameManager.Instance.AudioManager.Play("KnifeSFX");
        Destroy(kf, 2.0f);
    }

    public bool TryShoot()
    {
        ThrowKnife();
        return true;
    }
}
