using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class SoundSettingsInjectionData
    {
        [field: SerializeField] public SoundDataContainer[] Datas { get; private set; }
    }
}