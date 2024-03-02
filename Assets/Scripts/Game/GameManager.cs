using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Game
{
    /// <summary>
    /// Manages the game state. Put game session related logic here.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the <see cref="GameManager"/>.
        /// </summary>
        public static GameManager Instance { get; private set; }

        public BulletQueueManager BulletQueueManager => _bulletQueueManager;

        [SerializeField]
        private BulletQueueManager _bulletQueueManager;

        [SerializeField]
        private AnalyticsManager _analyticsManager;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            PlayerCharacterController.Instance.HealthComponent.OnDeath += OnPlayerDeath;
        }

        private void OnPlayerDeath(int newHealth)
        {
            StartCoroutine(EndGame());
        }

        private IEnumerator EndGame()
        {
            List<EnemyDamageKeyValue> damageDealtPerEnemyType = (
                from kvp in BulletQueueManager.DamageDealtPerEnemyType
                select new EnemyDamageKeyValue { Enemy = kvp.Key, Damage = kvp.Value }
            ).ToList();

            yield return _analyticsManager.LogRun(
                new RunData
                {
                    SurvivalTimeSeconds = (long)Time.time,
                    AmmoCollections = BulletQueueManager.AmmoCollections,
                    DamageDealtPerAmmo = BulletQueueManager.AmmoDamageDealt,
                    DamageDealtPerEnemyType = damageDealtPerEnemyType,
                }
            );
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
