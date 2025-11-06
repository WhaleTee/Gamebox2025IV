using UnityEngine;

public sealed class StateMachine : MonoBehaviour
{
    [Header("Начальный экран")]
    [Tooltip("Экран, который будет активирован при запуске.")]
    [SerializeField] private GameObject _firstScreen;

    /// <summary> Текущий активный экран. </summary>
    private GameObject _currentScreen;

    /// <summary> Блокировка повторной инициализации. </summary>
    private bool _initialized;

    private void Awake()
    {
        // Проверка корректности назначения.
        if (_firstScreen == null)
        {
            Debug.LogError($"[{nameof(StateMachine)}] Начальный экран не назначен в {gameObject.name}.", this);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Инициализирует стейт-машину и активирует первый экран.
    /// </summary>
    private void Initialize()
    {
        if (_initialized)
            return;

        ChangeState(_firstScreen);
        _initialized = true;
    }

    /// <summary>
    /// Меняет активное состояние на указанное.
    /// </summary>
    /// <param name="nextScreen">Экран, который станет активным.</param>
    public void ChangeState(GameObject nextScreen)
    {
        if (nextScreen == null)
        {
            Debug.LogWarning($"[{nameof(StateMachine)}] Попытка сменить состояние на null в {gameObject.name}.", this);
            return;
        }

        if (_currentScreen == nextScreen)
            return; // Никаких лишних операций, если экран тот же.

        // Деактивируем предыдущий экран
        if (_currentScreen != null && _currentScreen.activeSelf)
            _currentScreen.SetActive(false);

        // Активируем новый
        nextScreen.SetActive(true);
        _currentScreen = nextScreen;
    }

    /// <summary>
    /// Проверяет, является ли указанный экран текущим.
    /// </summary>
    public bool IsCurrent(GameObject screen) => _currentScreen == screen;
}
