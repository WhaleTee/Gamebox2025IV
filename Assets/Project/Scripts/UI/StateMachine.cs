using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace UI
{
    /// <summary>
    /// Управляет переключением экранов с плавной анимацией.
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        [Header("Начальный экран")]
        [Tooltip("Экран, который будет активирован при запуске.")]
        [SerializeField] private GameObject firstScreen;

        [Header("Настройки анимации")]
        [SerializeField, Min(0f)] private float fadeDuration = 0.3f;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;

        private GameObject _currentScreen;
        private CanvasGroup _currentGroup;
        private bool _isTransitioning;

        private void Start()
        {
            if (firstScreen == null)
            {
                Debug.LogError("Начальный экран не назначен.", this);
                return;
            }

            InitializeScreen(firstScreen);
        }

        /// <summary>
        /// Меняет активный экран на указанный.
        /// </summary>
        public void ChangeState(GameObject nextScreen)
        {
            if (_isTransitioning || nextScreen == null || nextScreen == _currentScreen)
                return;

            _ = TransitionAsync(nextScreen);
        }

        /// <summary>
        /// Асинхронно выполняет плавный переход между экранами.
        /// </summary>
        private async UniTask TransitionAsync(GameObject nextScreen)
        {
            _isTransitioning = true;

            CanvasGroup nextGroup = EnsureCanvasGroup(nextScreen);
            nextGroup.alpha = 0f;
            nextScreen.SetActive(true);

            // Скрываем текущий экран
            if (_currentGroup != null)
            {
                await _currentGroup
                    .DOFade(0f, fadeDuration)
                    .SetEase(fadeEase)
                    .SetUpdate(true)
                    .AsyncWaitForCompletion();

                _currentScreen.SetActive(false);
            }

            // Показываем новый экран
            await nextGroup
                .DOFade(1f, fadeDuration)
                .SetEase(fadeEase)
                .SetUpdate(true)
                .AsyncWaitForCompletion();

            _currentScreen = nextScreen;
            _currentGroup = nextGroup;

            _isTransitioning = false;
        }

        /// <summary>
        /// Гарантирует наличие CanvasGroup на экране.
        /// </summary>
        private CanvasGroup EnsureCanvasGroup(GameObject screen)
        {
            return screen.TryGetComponent(out CanvasGroup group)
                ? group
                : screen.AddComponent<CanvasGroup>();
        }

        /// <summary>
        /// Инициализация стартового экрана.
        /// </summary>
        private void InitializeScreen(GameObject screen)
        {
            CanvasGroup group = EnsureCanvasGroup(screen);
            screen.SetActive(true);
            group.alpha = 1f;

            _currentScreen = screen;
            _currentGroup = group;
        }
    }
}
