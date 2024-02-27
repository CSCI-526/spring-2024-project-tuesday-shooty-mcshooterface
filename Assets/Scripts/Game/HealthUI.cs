using Scripts.Player;
using TMPro;
using UnityEngine;

namespace Scripts.Game
{
    public class HealthUI : MonoBehaviour
    {
        private HealthComponent _healthComponent;
        public HealthComponent HealthComponent => _healthComponent;

        public TextMeshProUGUI healthText;

        // Start is called before the first frame update
        void Start()
        {
            _healthComponent = PlayerCharacterController.Instance.GetComponent<HealthComponent>();
            _healthComponent.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI(HealthComponent.CurrentHealth);
        }

        void UpdateHealthUI(int newHealth)
        {
            healthText.text = "Health: " + newHealth;
        }
    }
}
