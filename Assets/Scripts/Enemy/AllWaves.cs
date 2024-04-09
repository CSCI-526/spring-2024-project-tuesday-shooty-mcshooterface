using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllWaves", menuName = "AllWaves")]
public class AllWaves : ScriptableObject {
    public float timeBetweenWaves;
    public List<Wave> waves;
}

[Serializable]
public struct Wave {
    [Header("Wave Params")]
    public List<EnemyType> enemies;
}
