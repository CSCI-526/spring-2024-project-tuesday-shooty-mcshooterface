using System;
using System.Collections.Generic;

public static class TypeFinderUtility
{
    private static Dictionary<string, Type> _enemyTypes;

    public static IEnumerable<string> GetAllEnemyNames()
    {
        if (_enemyTypes == null)
        {
            _enemyTypes = new Dictionary<string, Type>();
            var types = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in types)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(EnemyAttribute), true).Length > 0)
                    {
                        _enemyTypes.Add(type.Name, type);
                    }
                }
            }
        }
        return _enemyTypes.Keys;
    }

    public static IEnumerable<Type> GetAllEnemyTypes()
    {
        if (_enemyTypes == null)
        {
            _enemyTypes = new Dictionary<string, Type>();
            var types = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in types)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(EnemyAttribute), true).Length > 0)
                    {
                        _enemyTypes.Add(type.Name, type);
                    }
                }
            }
        }
        return _enemyTypes.Values;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class EnemyAttribute : Attribute { }
