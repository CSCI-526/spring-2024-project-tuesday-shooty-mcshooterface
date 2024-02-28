using System;
using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class SwarmEnemyParent : MonoBehaviour
{
    [SerializeField] private GameObjectCollection enemyCollection;

    [Header("References")]
    [SerializeField] EnemyStatObject _enemyStatTunable;
    [SerializeField] SwarmEnemy swarmEnemy;

    [Header("Debug")]
    [SerializeField] List<SwarmEnemy> m_list;

    private void Awake()
    {
        m_list = new List<SwarmEnemy>();
        SpawnSwarm();
    }

    private void Start() {
        enemyCollection.Add(gameObject);
    }
    private void OnDestroy() {
        enemyCollection.Remove(gameObject);
    }

    void SpawnSwarm()
    {
        for (int i = 0; i < _enemyStatTunable.SwarmNumber; i++)
        {
            SwarmEnemy swarm = Instantiate(swarmEnemy, transform, false);
            m_list.Add(swarm);
            swarm.Construct(this);
        }
    }

    public void RemoveSwarmEnemy(SwarmEnemy swarm)
    {
        m_list.Remove(swarm);
        if (m_list.Count <= 0)
        {
            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.BulletQueueManager.Reload(_enemyStatTunable.SwarmDropColor);
        Destroy(gameObject);
    }
}
