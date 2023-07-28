using System;
using UnityEngine;

namespace Level
{
    [DisallowMultipleComponent]
    public sealed class Gate : MonoBehaviour
    {
        #region Fields
        private bool _isStopped;
        private Controller _controller = null;

        private const string PLAYER_TAG = "Player";
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _isStopped = false;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_isStopped) return;

            if (collision.CompareTag(PLAYER_TAG))
            {
                _controller.AddPoint();
            }
        }
        #endregion

        #region Public API
        public void Init(Controller controller)
        {
            _controller = controller;
        }

        internal void Stop() => _isStopped = true;
        #endregion
    }
}