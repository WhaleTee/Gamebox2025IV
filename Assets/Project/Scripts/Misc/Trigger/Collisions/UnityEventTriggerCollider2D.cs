using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Misc.Trigger.Collisions
{
    [RequireComponent(typeof(Collider2D))]
    public class UnityEventTriggerCollider2D : MonoBehaviour
    {
        [SerializeField] private LayerMask layers;
        [SerializeField] private bool triggerEnter;
        [SerializeField] private bool triggerStay;
        [SerializeField] private bool triggerExit;
        [SerializeField] private UnityEvent<Collider2D> onTriggerEnter;
        [SerializeField] private UnityEvent<Collider2D> onTriggerStay;
        [SerializeField] private UnityEvent<Collider2D> onTriggerExit;
        
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!triggerEnter) return;
            if (layers.Contains(other.gameObject.layer)) onTriggerEnter?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!triggerStay) return;
            if (layers.Contains(other.gameObject.layer)) onTriggerStay?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!triggerExit) return;
            if (layers.Contains(other.gameObject.layer)) onTriggerExit?.Invoke(other);
        }
    }
}