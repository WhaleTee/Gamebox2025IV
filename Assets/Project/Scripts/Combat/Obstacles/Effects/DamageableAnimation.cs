using System;
using UnityEngine;
using DG.Tweening;
using Extensions;
using Misc;

namespace Combat
{
    [Serializable]
    public class DamageableAnimation
    {
        [SerializeField] private DamageableAnimationConfig m_config;
        private Tween punchScale;
        private Tween punchPosition;
        private string tweenID;
        private GameObject gameObject;
        private Transform transform;

        public void Install(GameObject gameObject)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;

            tweenID = $"DamageableEffect_{gameObject.name}";
            punchScale = transform.DOPunchScale(Vector3.one * m_config.PunchScaleStrength, 0.2f, 30);
            punchScale.Preserve(tweenID);
        }

        public void PlayImpact(Vector2 point, Vector3 normal)
        {
            if (punchPosition != null && punchPosition.IsActive())
                punchPosition.Complete();
            punchPosition = transform.DOPunchPosition(-normal * m_config.PunchPositionStrength, 0.2f, 30).SetId(tweenID);
            PlayPunch(punchScale);
        }

        public async void DeathFadeOut(ActivateBase activate)
        {
            await DOTween.To(() => activate.Opacity, activate.SetOpacity, 0f, 0.3f).AsyncWaitForCompletion();
            activate.SetActive(false);
        }

        private void PlayPunch(Tween punch)
        {
            if (punch.IsActive())
                punch.Complete();
            punch.Restart();
        }
    }
}
