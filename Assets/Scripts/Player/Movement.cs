using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Movement : MonoBehaviour
    {
        #region Fields
        private Rigidbody2D _rigidbody2D = null;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
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

        #endregion
    }
}