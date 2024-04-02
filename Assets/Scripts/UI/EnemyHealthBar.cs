using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] protected Slider m_sliderBar;
    protected Camera m_camera;

    BaseEnemy m_enemy;

    public virtual void Construct(GameObject parent, Camera camera)
    {
        m_camera = camera;
        m_enemy = parent.GetComponent<BaseEnemy>();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    protected virtual void UpdateHealthBar()
    {
        if (m_enemy != null)
        {
            transform.position = m_enemy.transform.position;
            transform.rotation = m_camera.transform.rotation;

            m_sliderBar.value = m_enemy.HealthComponent.CurrentHealth * 1.0f / m_enemy.HealthComponent.MaxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
