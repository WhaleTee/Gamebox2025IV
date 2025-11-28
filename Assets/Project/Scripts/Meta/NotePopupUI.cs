using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class NotePopupUI : MonoBehaviour
{
    public static NotePopupUI Instance;

    [Header("UI")]
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private RectTransform root;
    [SerializeField] private TextMeshProUGUI textField;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float typingSpeed = 0.02f;
    [SerializeField] private float holdTime = 2f;

    private string fullText;
    private Tween currentTween;

    private void Awake()
    {
        Instance = this;

        panel.alpha = 0;
        root.anchoredPosition = new Vector2(0, -150);
        gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        fullText = text;
        _ = ShowAsync();   // fire & forget
    }

    private async UniTaskVoid ShowAsync()
    {
        gameObject.SetActive(true);
        textField.text = "";

        // На всякий случай убиваем старую анимацию
        currentTween?.Kill();

        // Появление панели
        root.DOAnchorPosY(0, 0.35f).SetEase(Ease.OutCubic);
        panel.DOFade(1f, fadeDuration);

        // Печать
        await Typewriter(fullText);

        // Ждем перед скрытием
        await UniTask.Delay((int)(holdTime * 1000));

        // Скрытие
        currentTween = panel.DOFade(0f, fadeDuration);
        root.DOAnchorPosY(-150, 0.3f).SetEase(Ease.InCubic);

        await currentTween.AsyncWaitForCompletion();

        gameObject.SetActive(false);
    }

    private async UniTask Typewriter(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textField.text = text.Substring(0, i + 1);
            await UniTask.Delay((int)(typingSpeed * 1000));
        }
    }
}
