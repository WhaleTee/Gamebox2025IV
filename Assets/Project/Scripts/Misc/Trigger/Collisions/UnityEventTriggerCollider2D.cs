using UnityEngine;
using UnityEngine.Events;

namespace Misc.Trigger.Collisions
{
    [System.Serializable]
    public class LayerTriggerEvent
    {
        [Tooltip("Слой, при взаимодействии с которым вызываются события")]
        public LayerMask layer;

        public bool triggerEnter = true;
        public bool triggerStay;
        public bool triggerExit;

        [Tooltip("Вызывается при входе в триггер")]
        public UnityEvent<Collider2D> onTriggerEnter;

        [Tooltip("Вызывается при нахождении в триггере")]
        public UnityEvent<Collider2D> onTriggerStay;

        [Tooltip("Вызывается при выходе из триггера")]
        public UnityEvent<Collider2D> onTriggerExit;
    }

    [RequireComponent(typeof(Collider2D))]
    public class UnityEventTriggerCollider2D : MonoBehaviour
    {
        [SerializeField] private LayerTriggerEvent[] layerEvents;
        private Collider2D _collider;

        private void Reset()
        {
            var col = GetComponent<Collider2D>();
            if (col != null)
                col.isTrigger = true;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other) =>
            InvokeEvents(other, e => e.triggerEnter, e => e.onTriggerEnter);

        private void OnTriggerStay2D(Collider2D other) =>
            InvokeEvents(other, e => e.triggerStay, e => e.onTriggerStay);

        private void OnTriggerExit2D(Collider2D other) =>
            InvokeEvents(other, e => e.triggerExit, e => e.onTriggerExit);

        private void InvokeEvents(Collider2D other, System.Func<LayerTriggerEvent, bool> predicate, System.Func<LayerTriggerEvent, UnityEvent<Collider2D>> eventSelector)
        {
            int otherLayerBit = 1 << other.gameObject.layer;

            foreach (var layerEvent in layerEvents)
            {
                if (!predicate(layerEvent)) continue;
                if ((layerEvent.layer.value & otherLayerBit) != 0)
                    eventSelector(layerEvent)?.Invoke(other);
            }
        }
    }
}
