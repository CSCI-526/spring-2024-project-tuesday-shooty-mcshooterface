using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void HealthEvent(int newHealth);

    public HealthEvent OnHealthChanged;
    public HealthEvent OnDeath;

    public int CurrentHealth => _currentHealth;

    [SerializeField]
    private int _startingHealth = 5;

    private int _currentHealth;

    void Start()
    {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int delta)
    {
        _currentHealth -= delta;
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth + delta > 0 && _currentHealth <= 0)
        {
            OnDeath?.Invoke(_currentHealth);
        }
    }

}
