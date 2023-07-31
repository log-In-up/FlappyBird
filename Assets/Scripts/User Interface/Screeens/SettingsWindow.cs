using Game;
using DifficultySystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public sealed class SettingsWindow : ScreenObserver
    {
        #region Editor fields
        [SerializeField] private Button _back = null;
        [SerializeField] private Slider _mainSound = null;
        [SerializeField] private Dropdown _difficultyDropdown = null;
        #endregion

        #region Fields
        private AudioSystem _audioSystem = null;
        private GameDifficultySystem _gameDifficultySystem;
        #endregion

        #region Properties
        public override UIScreen Screen => UIScreen.Settings;
        #endregion

        #region Overridden methods
        public override void Activate()
        {
            _difficultyDropdown.value = (int)_gameDifficultySystem.GetDifficultyData().DifficultyName;

            float mainSoundValue = _audioSystem.GetMasterVolume();

            _mainSound.value = Remap(mainSoundValue, -80.0f, 0.0f, _mainSound.minValue, _mainSound.maxValue);

            _back.onClick.AddListener(OnClickBack);
            _mainSound.onValueChanged.AddListener(OnChangeMainSound);
            _difficultyDropdown.onValueChanged.AddListener(OnChangeDifficulty);

            base.Activate();
        }

        public override void Deactivate()
        {
            _mainSound.onValueChanged.RemoveListener(OnChangeMainSound);
            _back.onClick.RemoveListener(OnClickBack);
            _difficultyDropdown.onValueChanged.RemoveListener(OnChangeDifficulty);

            base.Deactivate();
        }

        public override void Setup()
        {
            _audioSystem = Bootstrapper.Instance.AudioSystem;

            _gameDifficultySystem = Bootstrapper.Instance.GameDifficultySystem;

            Dropdown.OptionData easyDiffData = new Dropdown.OptionData(_gameDifficultySystem.GetDifficultyName(Difficulties.Easy));
            Dropdown.OptionData mediumDiffData = new Dropdown.OptionData(_gameDifficultySystem.GetDifficultyName(Difficulties.Medium));
            Dropdown.OptionData hardDiffData = new Dropdown.OptionData(_gameDifficultySystem.GetDifficultyName(Difficulties.Hard));

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>
            {
                easyDiffData,
                mediumDiffData,
                hardDiffData
            };
            _difficultyDropdown.AddOptions(optionDatas);

            base.Setup();
        }
        #endregion

        #region Methods
        private float Remap(float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }
        #endregion

        #region Event handlers
        private void OnClickBack()
        {
            UICore.OpenScreen(UIScreen.Menu);
        }

        private void OnChangeDifficulty(int argument)
        {
            Difficulties difficulties = (Difficulties)argument;

            _gameDifficultySystem.SetDifficulty(difficulties);
        }

        private void OnChangeMainSound(float value)
        {
            _audioSystem.SetMasterVolumeFromSlider(value);
        }
        #endregion
    }
}
