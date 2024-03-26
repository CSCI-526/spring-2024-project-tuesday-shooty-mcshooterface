using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyStatObject", order = 1)]
public class EnemyStatObject : ScriptableObject
{
    [Header("Ogre Variables")]
    [SerializeField] private int _ogreHealth;
    [SerializeField] private float _ogreSpeed;
    [SerializeField] private GameObject _ogreDrop;

    [Header("Swarm Variables")]
    [SerializeField] private int _swarmNumber;
    [SerializeField] private int _swarmHealth;
    [SerializeField] private float _swarmSpeed;
    [SerializeField] private GameObject _swarmDrop;

    [Header("Flying Variables")]
    [SerializeField] private int _flyingHealth;
    [SerializeField] private float _flyingSpeed;
    [SerializeField] private GameObject _flyingDrop;
    

    public int OgreHealth => _ogreHealth;
    public float OgreSpeed => _ogreSpeed;


    public int SwarmNumber => _swarmNumber;
    public int SwarmHealth => _swarmHealth;
    public float SwarmSpeed => _swarmSpeed;

    public int FlyingHealth => _flyingHealth;
    public float FlyingSpeed => _flyingSpeed;

    public int GetHealth(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Ogre:
                return _ogreHealth;
            case EnemyType.Swarm:
                return _swarmHealth;
            case EnemyType.Flying:
                return _flyingHealth;
        }

        return 0;
    }

    public GameObject GetEnemyDrop(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Ogre:
                return _ogreDrop;
            case EnemyType.Swarm:
                return _swarmDrop;
            case EnemyType.Flying:
                return _flyingDrop;
        }

        return null;
    }
}
