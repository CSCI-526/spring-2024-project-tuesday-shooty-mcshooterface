using UnityEngine;
using Scripts.Player;
using Scripts.Game;

public class Knife : MonoBehaviour, IGun
{
    // Start is called before the first frame update
    public GameObject KnifePrefab;
    public float KnifeSpeed = 30;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ThrowKnife() 
    {
        Transform spawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;

        GameObject kf = Instantiate(KnifePrefab, spawnTransform.position, spawnTransform.rotation) as GameObject;
        kf.GetComponent<Rigidbody>().velocity = spawnTransform.forward * KnifeSpeed;
        GameManager.Instance.AudioManager.Play("KnifeSFX");
        Destroy(kf, 2.0f);
    }

    public bool TryShoot()
    {
        ThrowKnife();
        return true;
    }
}
