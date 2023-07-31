using UnityEngine;
using Game;

namespace Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Movement : MonoBehaviour
    {
        #region Fields
        private float _jumpHeight;

        private GameDifficultySystem _dameDifficultySystem;
        private Rigidbody2D _rigidbody2D = null;
        private bool _isStopped;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _dameDifficultySystem = Bootstrapper.Instance.GameDifficultySystem;
        }

        private void Start()
        {
            _isStopped = true;
            _rigidbody2D.gravityScale = 0.0f;

            _jumpHeight = _dameDifficultySystem.GetDifficultyData().JumpHeight;
        }

        private void Update()
        {
            if (_isStopped) return;

            if (Input.GetMouseButtonDown(0))
            {
                MakeJump();
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase.Equals(TouchPhase.Ended))
                {
                    MakeJump();
                }
            }
        }
        #endregion

        #region Methods
        private void MakeJump()
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);
        }
        #endregion

        #region Public API
        public void Launch()
        {
            _rigidbody2D.gravityScale = 1.0f;
            _isStopped = false;

            _jumpHeight = _dameDifficultySystem.GetDifficultyData().JumpHeight;
        }

        public void Stop()
        {
            _isStopped = true;
            _rigidbody2D.gravityScale = 0.0f;
        }
        #endregion
    }
}