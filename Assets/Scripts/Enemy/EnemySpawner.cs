using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // [SerializeField] private bool spawnSwarmEnemy;
    [Header("Prefabs")]
    [SerializeField]
    private GameObjectCollection enemyCollection;

    [SerializeField]
    private GameObjectCollection meleeEnemyCollection;

    [SerializeField]
    private GameObjectCollection swarmEnemyCollection;

    [SerializeField]
    private GameObjectCollection flyingEnemyCollection;

    [SerializeField]
    private GameObject meleeEnemyPF;

    [SerializeField]
    private GameObject swarmEnemyParentPF;

    [SerializeField]
    private GameObject flyingEnemyPF;

    [Header("Spawn Locations")]
    [SerializeField]
    private List<Transform> spawnLocations;

    [Header("Random Spawning Params")]
    [SerializeField]
    private int maxEnemyCount;

    [SerializeField]
    private int maxMeleeEnemyCount;

    [SerializeField]
    private int maxSwarmEnemyCount;

    [SerializeField]
    private int maxFlyingEnemyCount;

    [SerializeField]
    private float timeBetweenSpawns;

    [Header("Wave Spawning Params")]
    [SerializeField]
    private AllWaves allWaves;

    // state
    private float _spawnTimer;
    private int _currentWave;
    private int _currentRandomSpawnAmount;

    // Start is called before the first frame update
    void Start()
    {
        _spawnTimer = allWaves.timeBetweenWaves;

        _currentWave = 0;
        SpawnCurrentWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentWave >= allWaves.waves.Count)
        {
            // random spawning
            UpdateRandomSpawning();
        }
        else
        {
            // wave spawning
            UpdateWaveSpawning();
        }
    }

    private void UpdateWaveSpawning()
    {
        if (enemyCollection.Count == 0)
        {
            _currentWave++;
            SpawnCurrentWave();
        }
    }

    private void SpawnCurrentWave()
    {
        if (_currentWave >= allWaves.waves.Count)
            return;

        Wave currentWave = allWaves.waves[_currentWave];

        foreach (var enemyType in currentWave.enemies)
        {
            SpawnEnemy(enemyType);
        }

        if (ToastUI.Instance)
        {
            var toastText = "Wave " + (_currentWave + 1);
            ToastUI.Instance.QueueToast(toastText);
        }
    }

    private void UpdateRandomSpawning()
    {
        if (_spawnTimer <= 0)
        {
            if (enemyCollection.Count < maxEnemyCount)
            {
                SpawnRandomEnemy();
                _spawnTimer = timeBetweenSpawns;
                if (++_currentRandomSpawnAmount >= maxEnemyCount)
                {
                    _currentWave++;
                    _currentRandomSpawnAmount = 0;
                }
            }
        }
        else
            _spawnTimer -= Time.deltaTime;
    }

    private Vector3 FarthestSpawnPosition()
    {
        // choose transform farthest from player
        float maxDistance = 0;
        var farthestSpawnLocation = transform;
        foreach (var location in spawnLocations)
        {
            var distance = Vector3.Distance(
                PlayerCharacterController.Instance.transform.position,
                location.transform.position
            );
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestSpawnLocation = location;
            }
        }
        return farthestSpawnLocation.position;
    }

    private void SpawnRandomEnemy()
    {
        // choose random enemy
        var enemiesToSpawn = new List<GameObject>();
        if (meleeEnemyCollection.Count < maxMeleeEnemyCount)
        {
            enemiesToSpawn.Add(meleeEnemyPF);
        }
        if (flyingEnemyCollection.Count < maxFlyingEnemyCount)
        {
            enemiesToSpawn.Add(flyingEnemyPF);
        }
        if (swarmEnemyCollection.Count < maxSwarmEnemyCount)
        {
            enemiesToSpawn.Add(swarmEnemyParentPF);
        }

        var randomIndex = Random.Range(0, enemiesToSpawn.Count);
        var newEnemyPrefab = enemiesToSpawn[randomIndex];

        // spawn enemy
        var newEnemy = Instantiate(newEnemyPrefab, FarthestSpawnPosition(), Quaternion.identity);
        if (newEnemy.TryGetComponent<SwarmEnemyParent>(out var swarmEnemyParent))
        {
            swarmEnemyParent.Wave = _currentWave;
        }
        else if (newEnemy.TryGetComponent<FlyingEnemy>(out var flyingEnemy))
        {
            flyingEnemy.Wave = _currentWave;
        }
        else if (newEnemy.TryGetComponent<OgreEnemy>(out var ogreEnemy))
        {
            ogreEnemy.Wave = _currentWave;
        }
    }

    private void SpawnEnemy(EnemyType type)
    {
        var enemyPF = GetEnemyPrefab(type);
        var newEnemy = Instantiate(enemyPF, FarthestSpawnPosition(), Quaternion.identity);

        if (newEnemy.TryGetComponent<SwarmEnemyParent>(out var swarmEnemyParent))
        {
            swarmEnemyParent.Wave = _currentWave;
        }
        else if (newEnemy.TryGetComponent<FlyingEnemy>(out var flyingEnemy))
        {
            flyingEnemy.Wave = _currentWave;
        }
        else if (newEnemy.TryGetComponent<OgreEnemy>(out var ogreEnemy))
        {
            ogreEnemy.Wave = _currentWave;
        }
    }

    private GameObject GetEnemyPrefab(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Flying:
                return flyingEnemyPF;
            case EnemyType.Ogre:
                return meleeEnemyPF;
            case EnemyType.Swarm:
                return swarmEnemyParentPF;
            default:
                return null;
        }
    }
}
