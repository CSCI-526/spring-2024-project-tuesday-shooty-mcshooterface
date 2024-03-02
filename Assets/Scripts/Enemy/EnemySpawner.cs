using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swarmEnemyParentPF;

    [SerializeField]
    private GameObject meleeEnemyParentPF;

    [SerializeField]
    private List<Transform> spawnLocations;

    [SerializeField]
    private GameObjectCollection enemyCollection;

    [SerializeField]
    private int maxEnemyCount;

    [SerializeField]
    private float spawnCooldown;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
