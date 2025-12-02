using System;
using UnityEngine;

namespace Misc
{
    public abstract class ObservableColliderBase<T> : MonoBehaviour, ICollider<T> where T : Collision2D
    {
        public event Action<T> OnEnter;
        public event Action<T> OnExit;

        protected void Enter(T t) => OnEnter?.Invoke(t);
        protected void Exit(T t) => OnExit?.Invoke(t);
    }
}