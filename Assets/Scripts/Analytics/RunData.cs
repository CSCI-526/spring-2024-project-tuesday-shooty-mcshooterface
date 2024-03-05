using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class RunData
{
    public long SurvivalTimeSeconds;

    public List<KeyValue<string, long>> AmmoCollections;

    public List<KeyValue<string, long>> DamageDealtPerAmmo;

    public List<KeyValue<string, long>> DamageDealtPerEnemyType;

    public string GetHeader()
    {
        AmmoCollections.Sort((x, y) => x.Key.CompareTo(y.Key));
        DamageDealtPerAmmo.Sort((x, y) => x.Key.CompareTo(y.Key));
        DamageDealtPerEnemyType.Sort((x, y) => x.Key.CompareTo(y.Key));
        StringBuilder sb = new StringBuilder();
        // header
        sb.Append("Survival Time (seconds),");
        foreach (KeyValue<string, long> kvp in AmmoCollections)
        {
            sb.Append(kvp.Key);
            sb.Append(',');
        }
        foreach (KeyValue<string, long> kvp in DamageDealtPerAmmo)
        {
            sb.Append(kvp.Key);
            sb.Append(',');
        }
        foreach (KeyValue<string, long> kvp in DamageDealtPerEnemyType)
        {
            sb.Append(kvp.Key);
            sb.Append(',');
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        AmmoCollections.Sort((x, y) => x.Key.CompareTo(y.Key));
        DamageDealtPerAmmo.Sort((x, y) => x.Key.CompareTo(y.Key));
        DamageDealtPerEnemyType.Sort((x, y) => x.Key.CompareTo(y.Key));

        StringBuilder sb = new StringBuilder();
        // data
        sb.Append(SurvivalTimeSeconds);
        sb.Append(',');
        foreach (KeyValue<string, long> kvp in AmmoCollections)
        {
            sb.Append(kvp.Value);
            sb.Append(',');
        }
        foreach (KeyValue<string, long> kvp in DamageDealtPerAmmo)
        {
            sb.Append(kvp.Value);
            sb.Append(',');
        }
        foreach (KeyValue<string, long> kvp in DamageDealtPerEnemyType)
        {
            sb.Append(kvp.Value);
            sb.Append(',');
        }

        return sb.ToString();
    }
}

[Serializable]
public class KeyValue<T1,T2> where T1 : class
{
    public T1 Key;
    public T2 Value;
}

