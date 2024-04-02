using System;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private GameObject swarmEnemyParentPF;
    [SerializeField] private GameObject meleeEnemyPF;
    [SerializeField] private GameObject flyingEnemyPF;

    [Header("Spawn Locations")]
    [SerializeField] private List<Transform> spawnLocations;
    
    [Header("Wave Spawning Params")] 
    [SerializeField] private SceneReference loadSceneAfterWaves;
    [SerializeField] private List<TutorialWave> waves;

    // state
    private int _currentWave;
    private bool _waveActive;
    private float _waveTimer;
    
    // Start is called before the first frame update
    void Start() {
        _currentWave = 0;
        _waveActive = false;
    }

    private void Update() {
        if (_waveActive) {
            _waveTimer += Time.deltaTime;
        }
        if (_waveActive && _waveTimer >= 2f && enemyCollection.Count == 0) {
            _waveActive = false;
            Debug.Log("Completed wave " + _currentWave);

            var completedWave = waves[_currentWave];
            if (completedWave.promptQueueOnComplete) {
                TutorialPromptManager.Instance.TryQueuePrompt(completedWave.promptQueueOnComplete);
            }
            
            _currentWave++;
            
            if(completedWave.nextWaveOnComplete) StartNextWave();
        }
    }

    public void StartNextWave() {
        if (_waveActive) return;
        if (_currentWave >= waves.Count) {
            SceneManager.LoadScene(loadSceneAfterWaves.ScenePath);
        }
        
        SpawnCurrentWave();
    }

    private void SpawnCurrentWave() {
        if (_currentWave >= waves.Count) return;
        
        TutorialWave currentWave = waves[_currentWave];

        foreach (var enemyType in currentWave.enemies) {
            SpawnEnemy(enemyType);
        }

        if (currentWave.promptQueueOnStart) {
            TutorialPromptManager.Instance.TryQueuePrompt(currentWave.promptQueueOnStart);
        }

        _waveActive = true;
        _waveTimer = 0;
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
struct TutorialWave {
    public List<EnemyType> enemies;
    public Prompt promptQueueOnStart;
    public Prompt promptQueueOnComplete;
    public bool nextWaveOnComplete;
}
