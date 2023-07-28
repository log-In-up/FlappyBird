using Level;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public sealed class GameWindow : ScreenObserver
    {
        #region Editor fields
        [SerializeField] private GameObject _loseMenu = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private Text _score = null;
        #endregion

        #region Fields
        private bool _isTouched;

        private Controller _controller;
        #endregion

        #region Properties
        public override UIScreen Screen => UIScreen.Game;
        #endregion

        #region MonoBehaviour API
        private void Update()
        {
            if (_isTouched) return;

            if (Input.touchCount > 0)
            {
                _controller.LaunchGame();

                _isTouched = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _controller.LaunchGame();

                _isTouched = true;
            }
        }
        #endregion

        #region Overridden methods
        public override void Activate()
        {
            _isTouched = false;

            _controller.Score += OnScoreChange;
            _controller.FallToGround += OnFallToGround;

            _closeButton.onClick.AddListener(OnClickClose);

            base.Activate();
        }

        public override void Deactivate()
        {
            _isTouched = false;
            _controller.ResetPlayer();
            _controller.ResetGenerator();

            _controller.Score -= OnScoreChange;
            _controller.FallToGround -= OnFallToGround;

            _closeButton.onClick.RemoveListener(OnClickClose);

            _loseMenu.SetActive(false);

            base.Deactivate();
        }

        public override void Setup()
        {
            _controller = FindObjectOfType<Controller>();

            base.Setup();
        }
        #endregion

        #region Event handlers
        private void OnScoreChange(int score)
        {
            _score.text = score.ToString();
        }

        private void OnFallToGround()
        {
            _loseMenu.SetActive(true);
        }

        private void OnClickClose()
        {
            _controller.ResetPoints();
            UICore.OpenScreen(UIScreen.Menu);
        }
        #endregion
    }
}