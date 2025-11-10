using UnityEngine;

/// <summary>
/// Класс для управления экранами в игре.
/// </summary>
public class StateMachine : MonoBehaviour
{
    [Header("Начальный экран")]
    [Tooltip("Экран, который будет активирован при запуске.")]
    [SerializeField] private GameObject _firstScreen;

    private GameObject _currentScreen;

    private void Start()
    {
        if (_firstScreen == null)
        {
            Debug.LogError("Начальный экран не назначен.", this);
            return;
        }

        ChangeState(_firstScreen);
    }

    /// <summary>
    /// Меняет активное состояние на указанное.
    /// </summary>
    /// <param name="nextScreen">Экран, который станет активным.</param>
    public void ChangeState(GameObject nextScreen)
    {
        if (nextScreen == null)
        {
            Debug.LogWarning("Попытка сменить состояние на null.", this);
            return;
        }

        if (_currentScreen != null)
        {
            _currentScreen.SetActive(false);
        }

        nextScreen.SetActive(true);
        _currentScreen = nextScreen;
    }
}