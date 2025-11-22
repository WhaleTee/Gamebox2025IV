using DG.Tweening;
using UnityEngine;

namespace Puzzle
{
    public class DoorOpen : MonoBehaviour, IMechanism
    {
        [SerializeField] private Transform door;
        [SerializeField] private Vector3 closedPositionLocal;
        [SerializeField] private Vector3 openPositionLocal;
        [SerializeField] private float closeDuration = 1.1f;
        [SerializeField] private float openDuration = 2.2f;
        [SerializeField] private ParticleSystem dustEffect;

        private Tween closeTween;
        private Tween openTween;
        private string tweenID;

        private void Awake()
        {
            tweenID = $"DoorOpen_{gameObject.name}";
        }

        public void Activate()
        {
            Debug.Log("Door opening");

            closeTween = door.DOLocalMove(openPositionLocal, closeDuration);
            openTween = door.DOLocalMove(closedPositionLocal, openDuration);
            PreserveTween(closeTween);
            PreserveTween(openTween);
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
