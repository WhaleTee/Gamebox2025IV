using System;
using UnityEngine;

namespace Misc
{
    public class ObservableTrigger : MonoBehaviour, ITrigger
    {
        public event Action<GameObject> OnEnter;
        public event Action<GameObject> OnExit;
        public event Action<GameObject> OnStay;

        private void OnTriggerEnter2D(Collider2D collision) => OnEnter?.Invoke(collision.gameObject);
        private void OnTriggerExit2D(Collider2D collision) => OnExit?.Invoke(collision.gameObject);
        private void OnTriggerStay2D(Collider2D collision) => OnStay?.Invoke(collision.gameObject);
    }
}