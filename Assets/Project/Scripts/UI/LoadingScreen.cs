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

        public async UniTask FadeInAsync()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            await Fade(0f, 1f);
            StartPulseEffect();
        }

        public async UniTask FadeOutAsync()
        {
            StopPulseEffect();
            await Fade(canvasGroup.alpha, 0f);
            gameObject.SetActive(false);
        }

        public void SetProgress(float value)
        {
            progressBar.value = value;
            if (progressText)
                progressText.text = $"Загрузка... {Mathf.RoundToInt(value * 100)}%";
        }

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

        private void StartPulseEffect()
        {
            if (!progressText) return;

            _pulseTween?.Kill();
            _pulseTween = progressText
                .DOFade(0.3f, pulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

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
