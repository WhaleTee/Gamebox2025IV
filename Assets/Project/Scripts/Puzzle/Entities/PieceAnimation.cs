using DG.Tweening;
using Extensions;
using System;
using UnityEngine;

namespace Puzzle.Entities
{
    [Serializable]
    public class PieceAnimation
    {
        [SerializeField] private SpriteRenderer m_mySprite;
        [SerializeField] private Sprite m_activated;
        [SerializeField] private Sprite m_activatedDamaged;
        [SerializeField] private Sprite m_intact;
        [SerializeField] private Sprite m_damaged;
        [SerializeField] private Sprite m_broken;

        [SerializeField] private float m_punchPositionStrength = 0.1f;
        [SerializeField] private float m_punchScaleStrength = -0.06f;

        private Tween punchPosition;
        private Tween punchScale;
        private string tweenID;

        public void OnValidate(GameObject gameObject)
        {
            if (m_mySprite == null)
                m_mySprite = gameObject.GetComponent<SpriteRenderer>();
        }

        public void Install(GameObject gameObject, string tweenID)
        {
            this.tweenID = tweenID;
            InstallTweens(gameObject.transform);
        }

        private void InstallTweens(Transform transform)
        {
            punchPosition = transform.DOPunchPosition(transform.right * m_punchPositionStrength, 0.2f, 30);
            punchScale = transform.DOPunchScale(Vector3.one * m_punchScaleStrength, 0.2f, 30);
            punchPosition.Preserve(tweenID);
            punchScale.Preserve(tweenID);
        }
    }
}