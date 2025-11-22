using System;
using UnityEngine;

namespace Misc
{
    public interface ITrigger<T> where T : Component
    {
        event Action<T> OnEnter;
        event Action<T> OnExit;
        event Action<T> OnStay;
    }
}