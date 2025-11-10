using UnityEngine;
using Input;

/// <summary>
/// Управляет постановкой и снятием паузы в игре.
/// </summary>
public class PauseManager : MonoBehaviour
{
    private bool _isPaused;
    public bool IsPaused => _isPaused;

    /// <summary>
    /// Ставит игру на паузу.
    /// </summary>
    public void PauseGame()
    {
        if (_isPaused) return;

        _isPaused = true;
        Time.timeScale = 0f;

        // Отключаем управление игроком
        if (UserInput.Instance != null)
            UserInput.Instance.enabled = false;
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
        if (UserInput.Instance != null)
            UserInput.Instance.enabled = true;
    }
}
