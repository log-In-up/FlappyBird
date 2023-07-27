using UnityEngine;

namespace LevelGenerator
{
    [DisallowMultipleComponent]
    public sealed class Destroyer : MonoBehaviour
    {
        #region Editor fields
        [SerializeField] private Generator _generator = null;
        #endregion

        #region Fields
        private const string PIPE_TAG = "Pipe";
        private const string GROUND_TAG = "Ground";
        #endregion

        #region MonoBehaviour API
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(PIPE_TAG))
            {
                _generator.DestroyPipe(collision.gameObject);
            }

            if (collision.collider.CompareTag(GROUND_TAG))
            {
                _generator.DestroyGroundTile(collision.gameObject);
            }
        }
        #endregion
    }
}