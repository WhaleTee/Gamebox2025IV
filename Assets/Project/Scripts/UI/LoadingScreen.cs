using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI progressText;

        [Header("Настройки")]
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float pulseDuration = 1.2f;

        private Tween _pulseTween;

        /// <summary>
        /// Показать LoadingScreen
        /// </summary>
        public async UniTask FadeInAsync()
        {
            gameObject.SetActive(true);

            canvasGroup.alpha = 0f;
            if (progressText) progressText.alpha = 1f;

            // Плавное появление
            await Fade(canvasGroup.alpha, 1f);

            // Запуск эффекта пульса текста
            StartPulseEffect();
        }

        /// <summary>
        /// Скрыть LoadingScreen
        /// </summary>
        public async UniTask FadeOutAsync()
        {
            StopPulseEffect();
            await Fade(canvasGroup.alpha, 0f);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Установить прогресс
        /// </summary>
        public void SetProgress(float value)
        {
            progressBar.value = value;
            if (progressText)
                progressText.text = $"loading... {Mathf.RoundToInt(value * 100)}%";
        }

        /// <summary>
        /// Плавное изменение альфы CanvasGroup
        /// </summary>
        private async UniTask Fade(float from, float to)
        {
            float time = 0f;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, time / fadeDuration);
                await UniTask.Yield();
            }
            canvasGroup.alpha = to;
        }

        /// <summary>
        /// Запуск мерцания текста
        /// </summary>
        private void StartPulseEffect()
        {
            if (!progressText) return;

            _pulseTween?.Kill();

            progressText.alpha = 1f;

            // DOTween пульсация текста
            _pulseTween = progressText
                .DOFade(0.3f, pulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// Остановка мерцания текста
        /// </summary>
        private void StopPulseEffect()
        {
            if (_pulseTween?.IsActive() == true)
            {
                _pulseTween.Kill();
                progressText.alpha = 1f;
            }
        }
    }
}
