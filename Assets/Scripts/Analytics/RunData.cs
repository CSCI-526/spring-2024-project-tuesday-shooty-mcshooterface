using System;
using System.Collections.Generic;

[Serializable]
public class RunData
{
    public long SurvivalTimeSeconds;

    public long[] AmmoCollections;

    public long[] DamageDealtPerAmmo;

    public List<EnemyDamageKeyValue> DamageDealtPerEnemyType;
}

[Serializable]
public class EnemyDamageKeyValue
{
    public string Enemy;
    public long Damage;
}
