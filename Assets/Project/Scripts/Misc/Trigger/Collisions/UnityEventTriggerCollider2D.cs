using Extensions;
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

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var layerEvent in layerEvents)
            {
                if (!layerEvent.triggerEnter) continue;
                if (layerEvent.layer.Contains(other.gameObject.layer))
                    layerEvent.onTriggerEnter?.Invoke(other);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            foreach (var layerEvent in layerEvents)
            {
                if (!layerEvent.triggerStay) continue;
                if (layerEvent.layer.Contains(other.gameObject.layer))
                    layerEvent.onTriggerStay?.Invoke(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            foreach (var layerEvent in layerEvents)
            {
                if (!layerEvent.triggerExit) continue;
                if (layerEvent.layer.Contains(other.gameObject.layer))
                    layerEvent.onTriggerExit?.Invoke(other);
            }
        }
    }
}
