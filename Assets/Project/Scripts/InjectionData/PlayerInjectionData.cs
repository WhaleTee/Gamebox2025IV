using System;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public class PlayerInjectionData
    {
        [field: SerializeField] public Transform PlayerStart { get; private set; }
        [field: SerializeField] public Hero Prefab { get; private set; }
        [field: SerializeField] public PlayerAbilities PlayerAbilities { get; private set; }
    }
}
