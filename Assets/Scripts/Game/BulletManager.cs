using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
    public List<Image> bullets;

    void Start()
    {
        foreach (var bullet in bullets)
        {
            bullet.enabled = true;
        }
    }

    public void Shoot()
    {
        foreach (var bullet in bullets)
        {
            if (bullet.enabled)
            {
                bullet.enabled = false;
                break;
            }
        }
    }
}

