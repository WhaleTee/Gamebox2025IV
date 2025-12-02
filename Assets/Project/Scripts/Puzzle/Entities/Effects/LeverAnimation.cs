using UnityEngine;
using DG.Tweening;
using Extensions;
using Puzzle.Entities;

namespace Puzzle
{
    public class LeverAnimation : PieceAnimation<LeverAnimationConfig>
    {
        [SerializeField] private Transform handle;
        [SerializeField] private Transform stick;
        private float delta;
        private Tween moveHandle;

        public override void Install(GameObject gameObject, PuzzleEvents events, PieceEvents selfEvents, string tweenID)
        {
            base.Install(gameObject, events, selfEvents, tweenID);

            moveHandle = DOTween.To(() => delta, v => delta = v, 1, m_config.Duration)
                .SetEase(m_config.Ease)
                .OnUpdate(HandleHandleMovement)
                .Preserve($"{tweenID}_MoveHandle");
        }

        public override void PlayImpact(Vector2 point, Vector3 normal, bool strong)
        {
            base.PlayImpact(point, normal, strong);
        }

        public override void PlayCharge()
        {
            base.PlayCharge();
            moveHandle.PlayForward();
            
        }

        public override void PlayDeactivate()
        {
            base.PlayDeactivate();
            moveHandle.PlayBackwards();
        }

        protected override Sprite SetSprite(PieceState state) =>
            state switch
            {
                _ when (state & PieceState.Charged) != 0 => m_config.Activated,
                _ => m_config.NotActive
            };

        private void HandleHandleMovement()
        {
            var pos = Vector3.Lerp(m_config.PositionOff, m_config.PositionOn, delta);
            var rotToStart = Quaternion.Lerp(m_config.RotationOff, m_config.RotationMiddlePoint, delta);
            var rotToEnd = Quaternion.Lerp(m_config.RotationMiddlePoint, m_config.RotationOn, delta);

            var rot = delta < 0.5 ? rotToStart : rotToEnd;

            stick.localRotation = rot;
            stick.localPosition = pos * 0.3f;
            handle.localPosition = pos;
        }

        protected override void Enable()
        {
            base.Enable();
            moveHandle.Play();
        }

        protected override void Disable()
        {
            base.Disable();
            if (moveHandle.IsActive() && moveHandle.IsPlaying())
                moveHandle.Pause();
        }
    }
}
