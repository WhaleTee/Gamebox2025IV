using UnityEngine;
using Input;
using Reflex.Attributes;
using System;

namespace Core
{
    /// <summary>
    /// Управляет постановкой и снятием паузы в игре.
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        [Inject] private UserInput userInput;

        private bool _isPaused;
        public bool IsPaused => _isPaused;

        public static event Action OnPauseRequested;

        private void OnEnable()
        {
            OnPauseRequested += PauseGame;
        }

        private void OnDisable()
        {
            OnPauseRequested -= PauseGame;
        }

        /// <summary>
        /// Ставит игру на паузу.
        /// </summary>
        public void PauseGame()
        {
            if (_isPaused) return;

            _isPaused = true;
            Time.timeScale = 0f;

            userInput.SetPlayerInputActive(false);
        }

        public void ResumeGame()
        {
            if (!_isPaused) return;

            _isPaused = false;
            Time.timeScale = 1f;

            userInput.SetPlayerInputActive(true);
        }


        /// <summary>
        /// Позволяет внешним скриптам запросить паузу через событие
        /// </summary>
        public static void RequestPause()
        {
            OnPauseRequested?.Invoke();
        }

    }
}
