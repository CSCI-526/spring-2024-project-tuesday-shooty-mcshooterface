using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmHealthBar : EnemyHealthBar
{
    SwarmEnemyParent m_swarmEnemy;

    public override void Construct(GameObject parent, Camera camera)
    {
        base.Construct(parent, camera);

        m_swarmEnemy = parent.GetComponent<SwarmEnemyParent>();
    }

    protected override void UpdateHealthBar()
    {
        if (m_swarmEnemy != null)
        {
            transform.position = m_swarmEnemy.SwarmCenter;
            transform.rotation = m_camera.transform.rotation;

            m_sliderBar.value = m_swarmEnemy.GetTotalCurrHP() * 1.0f / m_swarmEnemy.GetTotalMaxHP();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
