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

        // 🔥 Событие для внешних вызовов паузы
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

            // Отключаем управление игроком
            if (userInput != null)
                userInput.Enabled = false;
        }

        /// <summary>
        /// Возобновляет игру после паузы.
        /// </summary>
        public void ResumeGame()
        {
            if (!_isPaused) return;

            _isPaused = false;
            Time.timeScale = 1f;

            // Включаем управление игроком
            if (userInput != null)
                userInput.Enabled = true;
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
