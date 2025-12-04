using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class DistanceBasedVolume : MonoBehaviour
    {
        [SerializeField] private float maxDistance;
        private AudioSource audioSource;
        protected Transform target;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Update()
        {
            if (target == null) return;
            var distance = Vector3.Distance(target.position, transform.position);
            var volume = Mathf.Clamp01(1 - distance / maxDistance);
            audioSource.volume = volume;
        }
    }
}