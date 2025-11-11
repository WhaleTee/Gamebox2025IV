using System;
using UnityEngine;

namespace Misc
{
    [Serializable]
    public class SceneLifeCycle
    {
        public event Action OnAwake;
        public event Action OnStart;
        public void Awake() => OnAwake?.Invoke();
        public void Start() => OnStart?.Invoke();
    }
}
