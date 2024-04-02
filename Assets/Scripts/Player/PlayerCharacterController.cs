using System;
using Scripts.Game;
using UnityEngine;

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
        private GameObject _knifeModel;

        [SerializeField]
        private GameObject _launcherModel;

        [SerializeField]
        private GameObject _rifleModel;

        [SerializeField]
        private GameObject _shotgunModel;


        [SerializeField]
        private Transform _bulletSpawnTransform;

        private void Awake()
        {
            Instance = this;
            DisableAllModels();
            _knifeModel.SetActive(true); // TODO: Temp fix
        }

        private void DisableAllModels()
        {
            _knifeModel.SetActive(false);
            _launcherModel.SetActive(false);
            _rifleModel.SetActive(false);
            _shotgunModel.SetActive(false);
        }

        void Start()
        {
            _healthComponent = GetComponent<HealthComponent>();
            BulletQueueManager.Instance.OnBulletChanged += OnBulletChanged;
        }

        private void OnBulletChanged(BulletColor color)
        {
            GameObject _modelToActivate = color switch
            {
                BulletColor.Red => _launcherModel,
                BulletColor.Green => _shotgunModel,
                BulletColor.Blue => _rifleModel,
                BulletColor.Empty => _knifeModel,
                _ => throw new ArgumentOutOfRangeException(),
            };
            DisableAllModels();
            _modelToActivate.SetActive(true);
        }
    }
}
