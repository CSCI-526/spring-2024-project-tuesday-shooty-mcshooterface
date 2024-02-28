using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyStatObject", order = 1)]
public class EnemyStatObject : ScriptableObject
{
    [Header("Ogre Variables")]
    [SerializeField] private int _ogreHealth;
    [SerializeField] private float _ogreSpeed;
    [SerializeField] private BulletColor _ogreDropColor;

    [Header("Swarm Variables")]
    [SerializeField] private int _swarmNumber;
    [SerializeField] private int _swarmHealth;
    [SerializeField] private float _swarmSpeed;
    [SerializeField] private BulletColor _swarmDropColor;

    public int OgreHealth => _ogreHealth;
    public float OgreSpeed => _ogreSpeed;
    public BulletColor OgreDropColor => _ogreDropColor;

    public int SwarmNumber => _swarmNumber;
    public int SwarmHealth => _swarmHealth;
    public float SwarmSpeed => _swarmSpeed;
    public BulletColor SwarmDropColor => _swarmDropColor;

    public int GetHealth(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Ogre:
                return _ogreHealth;
            case EnemyType.Swarm:
                return _swarmHealth;
        }

        return 0;
    }

    public BulletColor GetDropColor(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Ogre:
                return _ogreDropColor;
            case EnemyType.Swarm:
                return _swarmDropColor;
        }
        return BulletColor.Empty;
    }
}
