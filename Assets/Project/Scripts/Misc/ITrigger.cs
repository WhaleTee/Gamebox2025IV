using System;
using UnityEngine;

namespace Misc
{
    public interface ITrigger
    {
        event Action<GameObject> OnEnter;
        event Action<GameObject> OnExit;
        event Action<GameObject> OnStay;
    }
}
