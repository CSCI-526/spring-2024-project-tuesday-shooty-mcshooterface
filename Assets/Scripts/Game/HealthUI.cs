using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private HealthComponent _healthComponent;
    public HealthComponent HealthComponent => (_healthComponent ??= GetComponent<HealthComponent>());

    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI(HealthComponent.CurrentHealth);
    }

    void UpdateHealthUI(int newHealth)
    {
        healthText.text = "Health: " + newHealth;
    }
}
