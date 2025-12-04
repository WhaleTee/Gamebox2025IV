using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactsMainMenuAnimate : MonoBehaviour
{
    [SerializeField] private Transform[] m_transforms;
    [SerializeField] private float m_duration = 3.5f;
    [SerializeField] private float m_height = 15f;
    [SerializeField] private Ease m_ease = Ease.InOutSine;
    private Dictionary<Transform, Tweener> tweens = new();
    private Vector3[] origins;
    private string tweenID = "TransformsMoveYWave";

    private void Start()
    {
        if (m_transforms == null || m_transforms.Length < 1)
            return;

        origins = new Vector3[m_transforms.Length];

        for (int i = 0; i < m_transforms.Length; i++)
        {
            var tr = m_transforms[i];
            origins[i] = tr.localPosition;
            Tweener tween = tr.DOLocalMoveY(m_height, m_duration);
            tween.SetDelay(CalcDelay(i)).SetLoops(-1, LoopType.Yoyo).SetRelative().SetId(tweenID);
            tweens.Add(tr, tween);
        }
    }

    private float CalcDelay(int i)
    {
        float t = i / (float)(m_transforms.Length - 1);
        float centerDist = Mathf.Abs(t * 2f - 1f);

        float eased = DOVirtual.EasedValue(
            0f,
            1f,
            centerDist,
            m_ease
        );

        return eased * m_duration;
    }

    private void OnDestroy()
    {
        foreach (var t in tweens.Values)
        {
            if (t.IsActive())
                t.Kill();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        UpdateInspectorChanges();
    }

    private void UpdateInspectorChanges()
    {
        int i = 0;
        foreach (var t in tweens.Values)
            t.ChangeEndValue(new Vector3(origins[i].x, m_height + origins[i].y, origins[i++].z), m_duration).SetEase(m_ease);
    }
#endif
}