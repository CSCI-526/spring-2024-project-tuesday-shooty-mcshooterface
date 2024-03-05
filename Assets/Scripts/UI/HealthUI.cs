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
            _healthComponent = PlayerHealthComponent.Instance;
            _healthComponent.OnDamageTaken += UpdateHealthUI;
            UpdateHealthUI((null, HealthComponent.CurrentHealth));
        }

        void UpdateHealthUI(in (DamageInfo _, int newHealth) args)
        {
            healthText.text = "Health: " + args.newHealth;
        }
    }
}
