using System;
using UnityEngine;

namespace Misc
{
    public abstract class ObservableTriggerBase<T> : MonoBehaviour, ITrigger<T> where T : Component
    {
        public event Action<T> OnEnter;
        public event Action<T> OnExit;
        public event Action<T> OnStay;

        protected void Enter(T t) => OnEnter?.Invoke(t);
        protected void Exit(T t) => OnExit?.Invoke(t);
        protected void Stay(T t) => OnStay?.Invoke(t);

    }
}