using System;
using System.Collections.Generic;

[Serializable]
public class RunData
{
    public long SurvivalTimeSeconds;

    public List<KeyValue<string, long>> AmmoCollections;

    public List<KeyValue<string, long>> DamageDealtPerAmmo;

    public List<KeyValue<string, long>> DamageDealtPerEnemyType;
}

[Serializable]
public class KeyValue<T1,T2> where T1 : class
{
    public T1 Key;
    public T2 Value;
}

