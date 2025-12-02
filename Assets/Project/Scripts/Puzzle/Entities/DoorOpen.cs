using UnityEngine;
using DG.Tweening;

namespace Puzzle
{
    public class DoorOpen : MonoBehaviour, IMechanism
    {
        [SerializeField] private PuzzleController puzzle;
        [SerializeField] private Transform door;
        [SerializeField] private Vector3 closedPositionLocal;
        [SerializeField] private Vector3 openPositionLocal;
        [SerializeField] private float closeDuration = 1.1f;
        [SerializeField] private float openDuration = 2.2f;
        [SerializeField] private ParticleSystem dustEffect;

        private Tween closeTween;
        private Tween openTween;
        private string tweenID;

        private void Start()
        {
            tweenID = $"DoorOpen_{gameObject.name}";
            puzzle.Events.Solved += Activate;

            closeTween = door.DOLocalMove(closedPositionLocal, closeDuration);
            openTween = door.DOLocalMove(openPositionLocal, openDuration);
            PreserveTween(closeTween);
            PreserveTween(openTween);
        }

        public void Activate()
        {

            float closed = Vector2.Distance(door.localPosition, closedPositionLocal);
            float open = Vector2.Distance(door.localPosition, openPositionLocal);

            Debug.Log($"Door opening - closed [{closed}] | open [{open}] | result [{closed < open}]");

            if (closed < open)
            {
                openTween.Restart();
                openTween.Play();
            }
            else
            {
                closeTween.Restart();
                closeTween.Play();
            }
            dustEffect.Play();
        }

        private void PreserveTween(Tween tween)
        {
            tween.SetAutoKill(false)
            .Pause()
            .SetRecyclable(false)
            .SetId(tweenID);
        }
    }
}
