using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Puzzle.Entities
{
    [Serializable]
    public abstract class PieceAnimation<TConfig> where TConfig : PuzzlePieceAnimationConfig
    {
        public TConfig Config => m_config;
        [SerializeField] protected TConfig m_config;
        [SerializeField] protected SpriteRenderer m_mySprite;
        [SerializeField] protected Light2D m_light;

        protected string tweenID;
        protected float cachedLightIntensity;
        protected float cachedLightOuterRadius;
        private Tween punchPosition;
        private Tween punchScale;

        protected Transform transform;
        protected PuzzleEvents events;
        protected PieceEvents selfEvents;

        public virtual void OnValidate(GameObject gameObject)
        {
            if (m_mySprite == null)
                m_mySprite = gameObject.GetComponent<SpriteRenderer>();
            if (m_light == null)
                m_light = gameObject.GetComponent<Light2D>();
        }

        public virtual void Install(GameObject gameObject, PuzzleEvents events, PieceEvents selfEvents, string tweenID)
        {
            this.transform = gameObject.transform;
            this.events = events;
            this.selfEvents = selfEvents;
            this.tweenID = tweenID;

            cachedLightIntensity = m_light.intensity;
            cachedLightOuterRadius = m_light.pointLightOuterRadius;
        }

        public void SetActive(bool value)
        {
            if (value)
                Enable();
            else
                Disable();
        }

        public virtual void PlayCharge()
        {
            m_light.enabled = true;
        }

        public virtual void PlayDischarged()
        {
            m_light.enabled = false;
        }

        public virtual void PlayDeactivate()
        {
            m_light.enabled = false;
        }

        public void SetState(PieceState state)
        {
            var sprite = SetSprite(state);
            m_mySprite.sprite = sprite;
        }

        public virtual void PlayImpact(Vector2 point, Vector3 normal, bool strong)
        {
            punchPosition = transform.DOPunchPosition(-normal * m_config.PunchPositionStrength, 0.2f, 30).SetId(tweenID);

            float s = strong ? 2.4f : 1;
            Vector3 scaleStrength = m_config.PunchScaleStrength * Vector3.one;
            punchScale = transform.DOPunchScale(scaleStrength, 0.2f, 30).SetId(tweenID);
        }

        protected void PlayReset()
        {
            DOTween.To(() => m_light.intensity, v => m_light.intensity = v, cachedLightIntensity, 1f)
                .SetEase(Ease.InOutSine).Play();
            DOTween.To(() => m_light.pointLightOuterRadius, v => m_light.pointLightOuterRadius = v, cachedLightOuterRadius, 0.5f)
                .SetEase(Ease.InOutSine).Play();
            m_light.enabled = false;
        }

        protected void PlaySolved()
        {
            DOTween.To(() => m_light.intensity, v => m_light.intensity = v, cachedLightIntensity * 0.4f, 0.6f)
                .SetEase(Ease.OutQuad).Play();
            DOTween.To(() => m_light.pointLightOuterRadius, v => m_light.pointLightOuterRadius = v, cachedLightOuterRadius * 6, 0.4f)
                .SetEase(Ease.InQuad).Play();
        }

        protected void PlayFailed()
        {
            DOTween.To(() => m_light.intensity, v => m_light.intensity = v, 0, 1f)
                .SetEase(Ease.InOutSine).Play();
            DOTween.To(() => m_light.pointLightOuterRadius, v => m_light.pointLightOuterRadius = v, cachedLightIntensity * 0.4f, 0.5f)
                .SetEase(Ease.InOutSine).Play();
            TurnOffLight(1);
        }

        //private Sprite SetSprite(PieceState state)
        //{
        //    if (state.Has(PieceState.Broken))
        //        return m_config.Broken;
        //    if (state.Has(PieceState.Charged) && state.Has(PieceState.DamagedAgain))
        //        return m_config.ChargedStronglyDamaged;
        //    if (state.Has(PieceState.Charged) && state.Has(PieceState.Damaged))
        //        return m_config.ChargedDamaged;
        //    if (state.Has(PieceState.Charged))
        //        return m_config.Charged;
        //    if (state.Has(PieceState.DamagedAgain))
        //        return m_config.StronglyDamaged;
        //    if (state.Has(PieceState.Damaged))
        //        return m_config.Damaged;
        //    return m_config.Intact;
        //}

        protected abstract Sprite SetSprite(PieceState state);


        private async void TurnOffLight(float delay)
        {
            await UniTask.Delay(Convert.ToInt32(delay * 1000));
            m_light.enabled = false;
        }

        protected virtual void SubAll()
        {
            selfEvents.StateChanged += SetState;
            events.Solved += PlaySolved;
            events.Reset += PlayReset;
            events.Failed += PlayFailed;
        }

        protected virtual void UnsubAll()
        {
            selfEvents.StateChanged -= SetState;
            events.Solved -= PlaySolved;
            events.Reset -= PlayReset;
            events.Failed -= PlayFailed;
        }

        protected virtual void Enable()
        {
            SubAll();

            if (punchScale.IsActive() && punchScale.IsPlaying())
                punchScale.Play();
            if (punchPosition.IsActive() && punchPosition.IsPlaying())
                punchPosition.Play();
        }

        protected virtual void Disable()
        {
            UnsubAll();

            if (punchScale.IsActive() && punchScale.IsPlaying())
                punchScale.Pause();
            if (punchPosition.IsActive() && punchPosition.IsPlaying())
                punchPosition.Pause();
        }
    }
}