using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectArchitecture;
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
        public AudioManager AudioManager => _audioManager;
        public ScoreManager ScoreManager => _scoreManager;
        public AnalyticsManager AnalyticsManager => _analyticsManager;
        public DeathManager DeathManager => _deathManager;

        [SerializeField]
        private BulletQueueManager _bulletQueueManager;

        [SerializeField]
        private AnalyticsManager _analyticsManager;

        [SerializeField]
        private AudioManager _audioManager;

        [SerializeField]
        private ScoreManager _scoreManager;

        [SerializeField]
        private DeathManager _deathManager;

        [SerializeField] protected IntVariable enemiesKilled;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            enemiesKilled.Value = 0;
        }

        
    }
}
