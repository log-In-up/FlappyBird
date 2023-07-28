using UnityEngine;

namespace Level
{
    [DisallowMultipleComponent]
    public sealed class Destroyer : MonoBehaviour
    {
        #region Editor fields
        [SerializeField] private Generator _generator = null;
        #endregion

        #region Fields
        private const string GROUND_TAG = "Ground";
        private const string PIPE_TAG = "Pipe";
        #endregion

        #region MonoBehaviour API
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Gate gate))
            {
                _generator.RemoveGateTile(gate);

                Destroy(collision.gameObject);
            }

            if (collision.collider.CompareTag(GROUND_TAG))
            {
                _generator.RemoveGroundTile(collision.gameObject);
                Destroy(collision.gameObject);
            }

            if (collision.collider.CompareTag(PIPE_TAG))
            {
                _generator.RemovePipe(collision.gameObject);
                Destroy(collision.gameObject);
            }
        }
        #endregion
    }
}