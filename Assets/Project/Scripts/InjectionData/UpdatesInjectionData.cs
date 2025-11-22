using UnityEngine;
using Misc;
using System;

namespace Assets.Project.Scripts.InjectionData
{
    [Serializable]
    public class UpdatesInjectionData
    {
        [field: SerializeField] public UpdateRunner UpdateRunner { get; private set; }
    }
}
