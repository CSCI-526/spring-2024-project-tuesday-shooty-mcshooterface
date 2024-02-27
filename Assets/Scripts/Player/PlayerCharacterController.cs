using System;
using Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Player
{
    public class PlayerCharacterController : MonoBehaviour {
        public static PlayerCharacterController Instance;
        public HealthComponent HealthComponent => _healthComponent ??= GetComponent<HealthComponent>();
        private HealthComponent _healthComponent;

        private void Awake() {
            Instance = this;
        }

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
