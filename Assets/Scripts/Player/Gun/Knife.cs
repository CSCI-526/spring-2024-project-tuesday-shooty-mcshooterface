using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using Scripts.Player.Gun;
using Scripts.Player;

public class Knife : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject KnifePrefab;
    public float KnifeSpeed = 30;
    public float cd = 1.0f; 

    private float _nextFireTime = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug
        if (Input.GetMouseButtonDown(1)) 
        {
            ThrowKnife();        
        }
    }

    public void ThrowKnife() 
    {
        Transform spawnTransform = PlayerCharacterController.Instance.BulletSpawnTransform;
        if (Time.time >= _nextFireTime) 
        {
            _nextFireTime = Time.time + cd;

            GameObject kf = Instantiate(KnifePrefab, spawnTransform.position, spawnTransform.rotation) as GameObject;
            kf.GetComponent<Rigidbody>().velocity = spawnTransform.forward * KnifeSpeed;

            Destroy(kf, 2.0f);
        }
    }
}
