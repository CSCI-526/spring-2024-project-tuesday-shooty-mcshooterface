using Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Player
{
    public class PlayerCharacterController : MonoBehaviour
    {
        public static PlayerCharacterController Instance;
        public HealthComponent HealthComponent =>
            _healthComponent ??= GetComponent<HealthComponent>();
        public Transform BulletSpawnTransform => _bulletSpawnTransform;
        private HealthComponent _healthComponent;

        [SerializeField]
        private Transform _bulletSpawnTransform;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _healthComponent = GetComponent<HealthComponent>();
        }
    }
}
