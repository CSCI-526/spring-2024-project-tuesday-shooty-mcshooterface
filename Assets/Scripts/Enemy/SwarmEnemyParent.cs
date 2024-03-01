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

    [SerializeField] private Vector3 _swarmCenter;

    [SerializeField] public Vector3 wanderDir;
    public Vector3 SwarmCenter 
    {
        get { return new Vector3(_swarmCenter.x, 0, _swarmCenter.z); }
    }

    private void Awake()
    {
        m_list = new List<SwarmEnemy>();
        SpawnSwarm();
    }

    private void Start() {
        enemyCollection.Add(gameObject);
        StartCoroutine(CheckPos()); 
    }


    IEnumerator CheckPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Vector3 t = new Vector3();
            for (int i = 0; i < m_list.Count; i++) 
            {
                t += m_list[i].transform.position;
            }
            _swarmCenter = t / m_list.Count;

            float a = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            wanderDir = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
        }

        
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
