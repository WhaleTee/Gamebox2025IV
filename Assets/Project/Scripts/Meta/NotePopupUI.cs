using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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

    private readonly Queue<string> queue = new();
    private bool isPlaying = false;

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
        queue.Enqueue(text);

        if (!isPlaying)
            _ = PlayQueueAsync();
    }
    private async UniTaskVoid PlayQueueAsync()
    {
        isPlaying = true;

        while (queue.Count > 0)
        {
            string text = queue.Dequeue();
            await PlaySingleNoteAsync(text);
        }

        isPlaying = false;
    }
    private async UniTask PlaySingleNoteAsync(string text)
    {
        gameObject.SetActive(true);
        textField.text = "";

        currentTween?.Kill();

        root.DOAnchorPosY(0, 0.35f).SetEase(Ease.OutCubic);
        panel.DOFade(1f, fadeDuration);

        await Typewriter(text);
        await UniTask.Delay((int)(holdTime * 1000));

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
