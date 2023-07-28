using UnityEngine;

namespace Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Movement : MonoBehaviour
    {
        #region Fields
        private Rigidbody2D _rigidbody2D = null;
        private bool _isStopped;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _isStopped = true;
            _rigidbody2D.gravityScale = 0.0f;
        }

        private void Update()
        {
            if(_isStopped) return;

            if(Input.GetMouseButtonDown(0))
            {
                MakeJump();
            }

            if (Input.touchCount > 0)
            {
                MakeJump();
            }
        }
        #endregion

        #region Methods
        private void MakeJump()
        {
            _rigidbody2D.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);
        }
        #endregion

        #region Public API
        public void Launch()
        {
            _rigidbody2D.gravityScale = 1.0f;
            _isStopped = false;
        }

        public void Stop()
        {
            _isStopped = true;
        }
        #endregion
    }
}