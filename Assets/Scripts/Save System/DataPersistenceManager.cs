using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
    [DisallowMultipleComponent]
    public sealed class DataPersistenceManager : MonoBehaviour
    {
        #region Editor fields
        [Header("File Storage Config")]
        [SerializeField] private string _fileName = "DataConfig.json";
        #endregion

        #region Fields
        private GameData _gameData = null;
        private List<IDataPersistence> _dataPersistenceObjects = null;
        private FileDataHandler _dataHandler = null;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);

            UpdatePersistenceObjects();
        }

        private void Start()
        {
            LoadGame();            
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
        #endregion

        #region Methods
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }
        #endregion

        #region Public methods
        public void UpdatePersistenceObjects()
        {
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load();

            if (_gameData == null)
            {
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(_gameData);
            }
        }

        public void NewGame()
        {
            _gameData = new GameData();

            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.NewGame(_gameData);
            }
        }

        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(_gameData);
            }

            _dataHandler.Save(_gameData);
        }
        #endregion
    }
}