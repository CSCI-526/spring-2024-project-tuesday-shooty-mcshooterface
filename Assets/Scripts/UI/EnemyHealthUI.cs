using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] Camera m_camera;
    [SerializeField] EnemyHealthBar m_flyingHealthBar;
    [SerializeField] EnemyHealthBar m_ogreHealthBar;
    [SerializeField] SwarmHealthBar m_swarmHealthBar;
    Dictionary<GameObject, EnemyHealthBar> m_enemyMapper = new Dictionary<GameObject, EnemyHealthBar>();
    public EnemyHealthBar CreateHealthBar(GameObject enemy)
    {
        if (enemy.GetComponent<OgreEnemy>() != null)
        {
            EnemyHealthBar healthbar = Instantiate(m_ogreHealthBar, transform);
            healthbar.Construct(enemy, m_camera);

            return healthbar;
        }
        else if (enemy.GetComponent<FlyingEnemy>() != null)
        {
            Debug.LogError("Tried to make flying enemy");
            EnemyHealthBar healthbar = Instantiate(m_flyingHealthBar, transform);
            healthbar.Construct(enemy, m_camera);

            return healthbar;
        }
        else if (enemy.GetComponent<SwarmEnemyParent>() != null)
        {
            SwarmHealthBar swarmbar = Instantiate(m_swarmHealthBar, transform);
            swarmbar.Construct(enemy, m_camera);

            return swarmbar;
        }

        return null;
    }

    private void Update()
    {
        foreach (GameObject e in enemyCollection)
        {
            if (!m_enemyMapper.ContainsKey(e))
            {
                EnemyHealthBar hp = CreateHealthBar(e);
                m_enemyMapper.Add(e, hp);
            }
        }

        CleanUpDictionary();
    }

    private void CleanUpDictionary()
    {
        List<GameObject> garbageCollection = new List<GameObject>();
        foreach (KeyValuePair<GameObject, EnemyHealthBar> kvp in m_enemyMapper)
        {
            if (kvp.Value == null)
            {
                garbageCollection.Add(kvp.Key);
            }
        }

        foreach (GameObject g in garbageCollection)
        {
            m_enemyMapper.Remove(g);
        }
    }
}
