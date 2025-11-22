using System;
using UnityEngine;

namespace Characters.Equipment
{
    [Serializable]
    public class Attach
    {
        [field: SerializeField] public Slot Slot { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }
    }
}