using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuButtonsAnimation : MonoBehaviour
{
    [SerializeField] private Image m_logo;
    [SerializeField] private RectTransform[] m_transforms;
    [SerializeField] private float m_duration = 0.7f;
    [SerializeField] private float m_delay = 0.6f;
    [SerializeField] private Vector3 m_startPosition = Vector3.up * -1000;
    [SerializeField] private Ease m_ease = Ease.OutQuad;
    private Vector3[] origins;
    private string tweenID = "MenuButtonsFadeInMove";

    private void Start()
    {
        if (m_transforms == null || m_transforms.Length < 1)
            return;

        origins = new Vector3[m_transforms.Length];

        for (int i = 0; i < origins.Length; i++)
            origins[i] = m_transforms[i].localPosition;

        Animate();
        AnimateLogo();
    }

    public void AnimateLogo()
    {
        var c = m_logo.color;
        m_logo.color = new Color(c.r, c.g, c.b, 0);
        m_logo.DOFade(1, m_duration * 1.5f).SetDelay(0.4f).SetEase(Ease.InQuad).SetTarget(this).SetId(tweenID);
    }

    public void Animate()
    {
        for (int i = 0; i < m_transforms.Length; i++)
        {
            var tr = m_transforms[i];
            tr.localPosition += m_startPosition;
            tr.DOLocalMoveY(origins[i].y, m_duration).SetDelay(m_delay * (i + 1.5f)).SetEase(m_ease).SetTarget(this).SetId(tweenID);
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}