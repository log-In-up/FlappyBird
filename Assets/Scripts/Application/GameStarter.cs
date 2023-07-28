using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    [DisallowMultipleComponent]
    public sealed class GameStarter : MonoBehaviour
    {
        #region Editor fields
        [SerializeField] private DataPersistenceManager _persistenceManager = null;
        #endregion

        #region MonoBehaviour API
        private void OnEnable()
        {
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        private void Start()
        {
            SceneManager.LoadScene(1);
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= ActiveSceneChanged;
        }
        #endregion

        #region Event handlers
        private void ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            _persistenceManager.UpdatePersistenceObjects();
        }
        #endregion
    }
}