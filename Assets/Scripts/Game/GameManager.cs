using Scripts.Player;
using StarterAssets;
using UnityEngine;

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

        [SerializeField] private BulletQueueManager _bulletQueueManager;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
