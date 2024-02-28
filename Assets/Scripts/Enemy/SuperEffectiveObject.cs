using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SuperEffectiveObject", order = 1)]
public class SuperEffectiveObject : ScriptableObject
{
    [Header("Ogre Effectiveness")]
    [SerializeField] private float rifleVersusOgreMultiplier;
    [SerializeField] private float shotgunVersusOgreMultiplier;
    [SerializeField] private float grenadeVersusOgreMultiplier;

    [Header("Swarm Effectiveness")]
    [SerializeField] private float rifleVersusSwarmMultiplier;
    [SerializeField] private float shotgunVersusSwarmMultiplier;
    [SerializeField] private float grenadeVersusSwarmMultiplier;

    [Header("Flying Effectiveness")]
    [SerializeField] private float rifleVersusFlyingMultiplier;
    [SerializeField] private float shotgunVersusFlyingMultiplier;
    [SerializeField] private float grenadeVersusFlyingMultiplier;


    public float GetMultiplier(EnemyType enemy, BulletColor color)
    {
        switch (enemy)
        {
            case EnemyType.Ogre:
                switch (color)
                {
                    case BulletColor.Red:
                        return grenadeVersusOgreMultiplier;
                    case BulletColor.Green:
                        return shotgunVersusOgreMultiplier;
                    case BulletColor.Blue:
                        return rifleVersusOgreMultiplier;
                }
                break;
            case EnemyType.Swarm:
                switch (color)
                {
                    case BulletColor.Red:
                        return grenadeVersusSwarmMultiplier;
                    case BulletColor.Green:
                        return shotgunVersusSwarmMultiplier;
                    case BulletColor.Blue:
                        return rifleVersusSwarmMultiplier;
                }
                break;
            case EnemyType.Flying:
                switch (color)
                {
                    case BulletColor.Red:
                        return grenadeVersusFlyingMultiplier;
                    case BulletColor.Green:
                        return shotgunVersusFlyingMultiplier;
                    case BulletColor.Blue:
                        return rifleVersusFlyingMultiplier;
                }
                break;
        }

        return 1.0f;
    }
}
