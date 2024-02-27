using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyParent : MonoBehaviour
{
    [Header("Swarm Enemy")]
    [SerializeField] int numSwarmers;
    [SerializeField] BulletColor _bulletColor;

    [Header("References")]
    [SerializeField] SwarmEnemy swarmEnemy;

    [Header("Debug")]
    [SerializeField] List<SwarmEnemy> m_list;

    private void Awake()
    {
        m_list = new List<SwarmEnemy>();
        SpawnSwarm();
    }

    void SpawnSwarm()
    {
        for (int i = 0; i < numSwarmers; i++)
        {
            SwarmEnemy swarm = Instantiate(swarmEnemy, transform, false);
            m_list.Add(swarm);
            swarm.Construct(this);
        }
    }

    public void RemoveSwarmEnemy(SwarmEnemy swarm)
    {
        m_list.Remove(swarm)
        if (m_list.Count <= 0)
        {
            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.BulletQueueManager.Reload(_bulletColor);
        Destroy(gameObject);
    }
}
