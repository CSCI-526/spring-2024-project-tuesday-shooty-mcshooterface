using System;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool spawnSwarmEnemy;
    [SerializeField]
    private GameObject swarmEnemyParentPF;

    [SerializeField] private bool spawnMeleeEnemy;
    [SerializeField]
    private GameObject meleeEnemyPF;

    [SerializeField] private bool spawnFlyingEnemy;
    [SerializeField]
    private GameObject flyingEnemyPF;

    [SerializeField]
    private List<Transform> spawnLocations;

    [SerializeField]
    private GameObjectCollection enemyCollection;

    [SerializeField]
    private int maxEnemyCount;

    [SerializeField]
    private float spawnTime;

    private float _spawnTimer;

    // Start is called before the first frame update
    void Start() {
        _spawnTimer = spawnTime;
    }

    // Update is called once per frame
    void Update() {
        if (_spawnTimer <= 0) {
            if (enemyCollection.Count < maxEnemyCount) {
                SpawnEnemy();
                _spawnTimer = spawnTime;   
            }
        }
        else _spawnTimer -= Time.deltaTime;
    }

    private void SpawnEnemy() {
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
        
        // choose random enemy
        var enemiesToSpawn = new List<GameObject>();
        if(spawnMeleeEnemy) enemiesToSpawn.Add(meleeEnemyPF);
        if(spawnFlyingEnemy) enemiesToSpawn.Add(flyingEnemyPF);
        if(spawnSwarmEnemy) enemiesToSpawn.Add(swarmEnemyParentPF);
        
        var randomIndex = Random.Range(0, enemiesToSpawn.Count);
        var newEnemyPrefab = enemiesToSpawn[randomIndex];
        
        // spawn enemy 
        Instantiate(newEnemyPrefab, farthestSpawnLocation.position, Quaternion.identity);
    }
}
