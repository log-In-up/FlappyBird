using UnityEngine;

namespace Player
{
    [DisallowMultipleComponent]
    public sealed class ObstacleDetector : MonoBehaviour
    {
        #region Fields
        private const string GROUND_TAG = "Ground", PIPE_TAG = "Pipe";
        #endregion

        #region Events
        public delegate void PerformDetectionObstacle();
        public event PerformDetectionObstacle OnDetectionObstacle, OnFallToGround;
        #endregion

        #region MonoBehaviour API
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(GROUND_TAG))
            {
                OnDetectionObstacle?.Invoke();
                OnFallToGround?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PIPE_TAG))
            {
                OnDetectionObstacle?.Invoke();
            }
        }
        #endregion
    }
}
