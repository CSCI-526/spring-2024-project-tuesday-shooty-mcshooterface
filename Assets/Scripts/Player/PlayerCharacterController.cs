using Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Player
{
    public class PlayerCharacterController : MonoBehaviour {

        public HealthComponent HealthComponent => _healthComponent ??= GetComponent<HealthComponent>();
        private HealthComponent _healthComponent;

        void Start()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.OnDeath += OnDeath;
        }

        private void OnDeath(int _)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
