using System;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // [SerializeField] private bool spawnSwarmEnemy;
    [Header("Prefabs")]
    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private GameObject swarmEnemyParentPF;
    [SerializeField] private GameObject meleeEnemyPF;
    [SerializeField] private GameObject flyingEnemyPF;

    [Header("Spawn Locations")]
    [SerializeField] private List<Transform> spawnLocations;
    
    [Header("Random Spawning Params")]
    [SerializeField] private int maxEnemyCount;

    [Header("Wave Spawning Params")] 
    [SerializeField] private float spawnTime;
    // [SerializeField] private float timeBetweenWaves;
    [SerializeField] private List<Wave> waves;
    
    // state
    private float _spawnTimer;
    private int _currentWave;
    private float _waveTimer;

    // Start is called before the first frame update
    void Start() {
        _spawnTimer = spawnTime;
        
        _currentWave = 0;
        SpawnCurrentWave();
    }

    // Update is called once per frame
    void Update() {
        if (_currentWave >= waves.Count) {
            // random spawning
            UpdateRandomSpawning();
        }
        else {
            // wave spawning
            UpdateWaveSpawning();
        }
    }

    private void UpdateWaveSpawning() {
        _waveTimer -= Time.deltaTime;

        if (_waveTimer <= 0) {
            _currentWave++;
            SpawnCurrentWave();
        }
        else if(enemyCollection.Count == 0) {
            _currentWave++;
            SpawnCurrentWave();
        }
    }

    private void SpawnCurrentWave() {
        if (_currentWave >= waves.Count) return;
        
        Wave currentWave = waves[_currentWave];

        foreach (var enemyType in currentWave.enemies) {
            SpawnEnemy(enemyType);
        }

        _waveTimer = currentWave.maxTime;
    }

    private void UpdateRandomSpawning() {
        if (_spawnTimer <= 0) {
            if (enemyCollection.Count < maxEnemyCount) {
                SpawnRandomEnemy();
                _spawnTimer = spawnTime;   
            }
        }
        else _spawnTimer -= Time.deltaTime;
    }

    private Vector3 FarthestSpawnPosition() {
        // choose transform farthest from player
        float maxDistance = 0;
        var farthestSpawnLocation = transform;
        foreach (var location in spawnLocations) {
            var distance = Vector3.Distance(PlayerCharacterController.Instance.transform.position, location.transform.position);
            if (distance > maxDistance) {
                maxDistance = distance;
                farthestSpawnLocation = location;
            }
        }
        return farthestSpawnLocation.position;
    }

    private void SpawnRandomEnemy() {
        // choose random enemy
        var enemiesToSpawn = new List<GameObject>();
        enemiesToSpawn.Add(meleeEnemyPF);
        enemiesToSpawn.Add(flyingEnemyPF);
        enemiesToSpawn.Add(swarmEnemyParentPF);
        
        var randomIndex = Random.Range(0, enemiesToSpawn.Count);
        var newEnemyPrefab = enemiesToSpawn[randomIndex];
        
        // spawn enemy 
        Instantiate(newEnemyPrefab, FarthestSpawnPosition(), Quaternion.identity);
    }

    private void SpawnEnemy(EnemyType type) {
        var enemyPF = GetEnemyPrefab(type);
        Instantiate(enemyPF, FarthestSpawnPosition(), Quaternion.identity);
    }

    private GameObject GetEnemyPrefab(EnemyType type) {
        switch (type) {
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

[Serializable]
struct Wave {
    public float maxTime;
    public List<EnemyType> enemies;
}
