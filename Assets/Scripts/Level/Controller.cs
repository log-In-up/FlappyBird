using Player;
using UnityEngine;

namespace Level
{
    [DisallowMultipleComponent]
    public sealed class Controller : MonoBehaviour
    {
        #region Editor fields
        [SerializeField] private GameObject _player = null;
        [SerializeField] private Generator _generator = null;
        #endregion

        #region Fields
        private Vector2 _playerStartPositon;
        private ObstacleDetector _obstacleDetector = null;
        private Movement _movement = null;

        private int _score;
        #endregion

        #region Events
        public delegate void PerformScore(int score);
        public event PerformScore Score;

        public delegate void PerformDetectionGround();
        public event PerformDetectionGround FallToGround;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _playerStartPositon = _player.transform.position;

            _obstacleDetector = FindObjectOfType<ObstacleDetector>(true);
            
            _movement = FindObjectOfType<Movement>(true);
        }

        private void OnEnable()
        {
            _obstacleDetector.OnDetectionObstacle += StopGame;
            _obstacleDetector.OnFallToGround += OnFallToGround;
        }

        private void OnDisable()
        {
            _obstacleDetector.OnDetectionObstacle -= StopGame;
            _obstacleDetector.OnFallToGround -= OnFallToGround;
        }
        #endregion

        #region Event handlers
        private void StopGame()
        {
            _movement.Stop();
            _generator.Stop();
        }

        private void OnFallToGround() => FallToGround?.Invoke();
        #endregion

        #region Public API
        internal void AddPoint()
        {
            _score++;
            Score?.Invoke(_score);
        }

        public void ShowPlayer() => _player.SetActive(true);

        public void LaunchGame()
        {
            _movement.Launch();
            _generator.Launch();
        }

        public void ResetPlayer()
        {
            _player.SetActive(false);
            _player.transform.position = _playerStartPositon;
        }

        public void ResetGenerator() => _generator.ResetGenerator();

        public void ResetPoints()
        {
            _score = 0;
            Score?.Invoke(_score);
        }
        #endregion
    }
}