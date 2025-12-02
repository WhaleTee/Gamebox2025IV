using System;
using UnityEngine;

namespace Misc
{
    public interface ICollider<T> where T : Collision2D
    {
        event Action<T> OnEnter;
        event Action<T> OnExit;
    }
}