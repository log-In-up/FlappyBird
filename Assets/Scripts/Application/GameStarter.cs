using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    [DisallowMultipleComponent]
    public sealed class GameStarter : MonoBehaviour
    {
        #region MonoBehaviour API
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
        #endregion
    }
}