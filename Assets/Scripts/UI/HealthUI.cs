using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class HealthUI : MonoBehaviour
    {
        private HealthComponent _healthComponent;
        public HealthComponent HealthComponent => _healthComponent;

        public TextMeshProUGUI healthText;
        public Image[] healthBars;

        // Start is called before the first frame update
        void Start()
        {
            _healthComponent = PlayerHealthComponent.Instance;
            _healthComponent.OnDamageTaken += UpdateHealthUI;
            _healthComponent.OnHealed += UpdateHealthUI;
            UpdateHealthUI((null, HealthComponent.CurrentHealth));
        }

        void UpdateHealthUI(in (DamageInfo _, int newHealth) args)
        {
            UpdateHealthBar(args.newHealth);
        }

        void UpdateHealthUI(in (int healing, int newHealth) args)
        {
            UpdateHealthBar(args.newHealth);
        }

        void UpdateHealthBar(int newHealth)
        {
            int healthBarsCount = healthBars.Length;
            newHealth = Mathf.Min(newHealth, healthBarsCount);

            for (int i = 0; i < healthBars.Length; i++)
            {
                healthBars[i].gameObject.SetActive(i < newHealth);
            }
        }
    }
}
