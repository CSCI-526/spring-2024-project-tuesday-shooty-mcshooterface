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

        private void DisableAllModels(GameObject modelToKeep = null)
        {
            if (modelToKeep != _knifeModel) _knifeModel.SetActive(false);
            if (modelToKeep != _launcherModel) _launcherModel.SetActive(false);
            if (modelToKeep != _rifleModel) _rifleModel.SetActive(false);
            if (modelToKeep != _shotgunModel) _shotgunModel.SetActive(false);
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
            DisableAllModels(_modelToActivate);
            _modelToActivate.SetActive(true);
        }
    }
}
