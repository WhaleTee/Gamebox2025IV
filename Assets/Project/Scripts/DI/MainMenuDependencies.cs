using System;
using UnityEngine;
using Sound;

namespace DI
{
    [Serializable]
    public class MainMenuDependencies : MonoBehaviour
    {
        [field: SerializeField] public SoundSettingsInjectionData SoundSettingsData { get; private set; }
    }
}