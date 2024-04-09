using System;
using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using UnityEngine;


public class SwarmEnemyParent : MonoBehaviour
{
    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private GameObjectCollection swarmEnemyCollection;

    [Header("References")]
    [SerializeField] EnemyStatObject _enemyStatTunable;
    [SerializeField] SwarmEnemy swarmEnemy;
    [SerializeField] protected IntVariable enemiesKilled;
    [SerializeField] Transform _spawnPoint;

    [Header("Debug")]
    [SerializeField] List<SwarmEnemy> m_list;

    [SerializeField] private Vector3 _swarmCenter;

    [SerializeField] public Vector3 wanderDir;

    public float attackCD = 5.0f;
    public float attackIndicate = 1.8f;
    public GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;

    // sight detect
    private bool _insight = true;
    private float _fov = 90;

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
        swarmEnemyCollection.Add(gameObject);
        StartCoroutine(CheckPos());
        StartCoroutine(SetAttack());
        _swarmCenter = transform.position;
    }

    private void Update()
    {
        if (m_list.Count > 0)
        {
            Vector3 t = new Vector3();
            for (int i = 0; i < m_list.Count; i++)
            {
                t += m_list[i].transform.position;
            }
            _swarmCenter = t / m_list.Count;
        }

        // update _insight
        Transform pt = PlayerCharacterController.Instance.transform;
        float angle = Vector3.Angle(pt.forward, _swarmCenter - pt.position);
        _insight = angle < (_fov / 2) ? true : false;
    }

    IEnumerator CheckPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            float a = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            wanderDir = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
        }
    }

    IEnumerator SetAttack() 
    {
        while (true) 
        {
            if (!_insight)
            {
                Debug.Log("Swarm: Attack canceled!");

            }
            else 
            {
                for (int i = 0; i < m_list.Count; i++)
                {
                    m_list[i].IndicateAttack();
                }
                yield return new WaitForSeconds(attackIndicate);
                Attack();

            }
            yield return new WaitForSeconds(attackCD);
        }
    }

    private void Attack() 
    {
        Transform pt = PlayerCharacterController.Instance.transform;
        GameObject projectile = Instantiate(projectilePrefab, _swarmCenter, transform.rotation) as GameObject;

        float offset = 1.0f;
        Vector3 s2p = (pt.position - _swarmCenter);
        Vector3 aim = new Vector3(s2p.x, s2p.y + offset, s2p.z);

        projectile.GetComponent<Rigidbody>().AddForce(aim * projectileSpeed, ForceMode.Impulse);
        Destroy(projectile, 5.0f);
    }

    private void OnDestroy() {
        enemyCollection.Remove(gameObject);
        swarmEnemyCollection.Remove(gameObject);
    }

    void SpawnSwarm()
    {
        for (int i = 0; i < _enemyStatTunable.SwarmNumber; i++)
        {
            SwarmEnemy swarm = Instantiate(swarmEnemy, _spawnPoint, false);
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
        GameManager.Instance.ScoreManager.GiveEnemyBonus();
        SpawnDrop(_swarmCenter);
        enemiesKilled.Value++;
        Destroy(gameObject);
    }

    protected virtual void SpawnDrop(Vector3 spawnPosition)
    {
        GameObject drop = Instantiate(_enemyStatTunable.GetEnemyDrop(EnemyType.Swarm), spawnPosition, Quaternion.identity);
        drop.transform.position = new Vector3(drop.transform.position.x, 0, drop.transform.position.z);
    }

    public int GetTotalMaxHP()
    {
        return _enemyStatTunable.SwarmNumber * _enemyStatTunable.SwarmHealth;
    }
    public int GetTotalCurrHP()
    {
        int output = 0;
        foreach (SwarmEnemy s in m_list)
        {
            output += s.HealthComponent.CurrentHealth;
        }
        return output;
    }
}
