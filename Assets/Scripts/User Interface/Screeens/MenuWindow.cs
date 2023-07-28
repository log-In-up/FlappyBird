using Level;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public sealed class MenuWindow : ScreenObserver
    {
        #region Editor fields
        [SerializeField] private Button _settings = null;
        [SerializeField] private Button _start = null;
        #endregion

        #region Fields
        private Controller _controller = null;
        #endregion

        #region Properties
        public override UIScreen Screen => UIScreen.Menu;
        #endregion

        #region Overridden methods
        public override void Activate()
        {
            _settings.onClick.AddListener(OnClickSettings);
            _start.onClick.AddListener(OnClickStart);

            base.Activate();
        }

        public override void Deactivate()
        {
            _settings.onClick.RemoveListener(OnClickSettings);
            _start.onClick.RemoveListener(OnClickStart);

            base.Deactivate();
        }

        public override void Setup()
        {
            _controller = FindObjectOfType<Controller>();

            base.Setup();
        }
        #endregion

        #region Event handlers
        private void OnClickSettings()
        {
            UICore.OpenScreen(UIScreen.Settings);
        }

        private void OnClickStart()
        {
            UICore.OpenScreen(UIScreen.Game);

            _controller.ShowPlayer();
        }
        #endregion
    }
}