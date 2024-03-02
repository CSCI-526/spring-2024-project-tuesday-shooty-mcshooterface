using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletQueueManager : MonoBehaviour
{
    private Queue<BulletColor> bulletQueue = new Queue<BulletColor>();
    private Dictionary<string, long> _ammoCollections = new();
    private Dictionary<string, long> _ammoDamageDealt = new();
    private Dictionary<string, long> _damageDealtPerEnemyType = new();

    public BulletColor Top
    {
        get => bulletQueue.Count == 0 ? BulletColor.Empty : bulletQueue.Peek();
    }

    /// <summary>
    /// The ammo collections of the player. For analytics purposes.
    /// </summary>
    public Dictionary<string, long> AmmoCollections => _ammoCollections;

    /// <summary>
    /// The damage dealt per ammo type. For analytics purposes.
    /// </summary>
    public Dictionary<string, long> AmmoDamageDealt => _ammoDamageDealt;

    /// <summary>
    /// The damage dealt per enemy type. For analytics purposes.
    /// </summary>
    public Dictionary<string, long> DamageDealtPerEnemyType => _damageDealtPerEnemyType;

    public BulletQueueUI bulletQueueUI;

    private void Start()
    {
        // initialization (all of bullets are set to empty)
        for (int i = 0; i < 6; i++)
        {
            bulletQueue.Enqueue(BulletColor.Empty);
        }

        foreach (BulletColor color in Enum.GetValues(typeof(BulletColor)))
        {
            _ammoCollections[color.ToString()] = 0;
            _ammoDamageDealt[color.ToString()] = 0;
        }

        // TODO: Add enemy types

        bulletQueueUI.UpdateQueueDisplay(bulletQueue);
    }

    private void Update()
    {
        // Should be related to the shoot function in GameManager
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        */

        // Clear all of the queue for debug
        if (Input.GetKeyDown(KeyCode.H))
        {
            ClearQueue();
        }

        // Reload with three kinds of bullets
        // Debug Mode:
        // J key to reload Red bullet
        // K key to reload Green bullet
        // L key to reload Blue bullet
        // H key for clearing queue
        if (Input.GetKeyDown(KeyCode.J))
        {
            Reload(BulletColor.Red);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Reload(BulletColor.Green);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Reload(BulletColor.Blue);
        }
    }

    public BulletColor LoseAmmo()
    {
        if (bulletQueue.Count > 0 && bulletQueue.Peek() != BulletColor.Empty)
        {
            BulletColor output = bulletQueue.Peek();

            bulletQueue.Dequeue();
            bulletQueue.Enqueue(BulletColor.Empty);
            bulletQueueUI.UpdateQueueDisplay(bulletQueue);

            return output;
        }

        return BulletColor.Empty;
    }

    void ClearQueue()
    {
        bulletQueue.Clear();
        for (int i = 0; i < 6; i++)
        {
            bulletQueue.Enqueue(BulletColor.Empty);
        }
        bulletQueueUI.UpdateQueueDisplay(bulletQueue);
    }

    public void Reload(BulletColor color)
    {
        List<BulletColor> tempList = new List<BulletColor>(bulletQueue);
        int index = tempList.IndexOf(BulletColor.Empty);
        if (index != -1)
        {
            tempList[index] = color;
            bulletQueue = new Queue<BulletColor>(tempList);
            bulletQueueUI.UpdateQueueDisplay(bulletQueue);
        }
    }
}

public enum BulletColor
{
    Red,
    Green,
    Blue,
    Empty
}
