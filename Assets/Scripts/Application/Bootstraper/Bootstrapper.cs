using DifficultySystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Application
{
    [DisallowMultipleComponent]
    public sealed class Bootstrapper : MonoBehaviour, IDataPersistence
    {
        #region Editor field
        [SerializeField] private DifficultyDataLoader _difficultyDataLoader = null;
        [SerializeField] private AudioMixer _audioMixer = null;
        #endregion

        #region Fields
        private AudioSystem _audioSystem = null;
        private GameDifficultySystem _gameDifficultySystem = null;
        #endregion

        #region Properties
        public GameDifficultySystem GameDifficultySystem => _gameDifficultySystem;
        public AudioSystem AudioSystem => _audioSystem;

        public static Bootstrapper Instance { get; private set; }
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            _audioSystem = new AudioSystem(_audioMixer);
            _gameDifficultySystem = new GameDifficultySystem(_difficultyDataLoader);
        }

        public void LoadData(GameData data)
        {
            Difficulties difficulty = (Difficulties)data.DifficultyLevel;
            _gameDifficultySystem.SetDifficulty(difficulty);

            _audioSystem.SetMasterVolume(data.MasterVolume);
        }

        public void NewGame(GameData data)
        {
            data.DifficultyLevel = 0;
            _gameDifficultySystem.SetDifficulty(Difficulties.Easy);

            data.MasterVolume = 0;
            _audioSystem.SetMasterVolume(data.MasterVolume);
        }

        public void SaveData(GameData data)
        {
            data.DifficultyLevel = (int)_gameDifficultySystem.GetDifficultyData().DifficultyName;

            data.MasterVolume = _audioSystem.GetMasterVolume();
        }
        #endregion
    }
}