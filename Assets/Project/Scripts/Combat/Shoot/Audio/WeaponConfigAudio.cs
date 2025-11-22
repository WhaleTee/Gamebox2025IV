using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Weapon
{
    [CreateAssetMenu(fileName = "WeaponAudioConfig_01", menuName = "Scriptables/Combat/Weapon/Effects/Audio")]
    public class WeaponConfigAudio : ScriptableObject
    {
        [field: SerializeField] public int PolyphonyLimit { get; protected set; } = 3;
        [field: SerializeField] public AudioClip Shot { get; protected set; }
        [field: SerializeField] public AudioClip Exhausted { get; protected set; }
        [field: SerializeField] public AudioClip Cancel { get; protected set; }
        [field: SerializeField] public AudioClip Start { get; protected set; }
        [field: SerializeField] public AudioClip Finish { get; protected set; }
        public Dictionary<SoundType, AudioClip> Sounds;

        public void OnEnable()
        {
            Sounds = new() {
        { SoundType.Shot, Shot },
        { SoundType.Exhausted, Exhausted },
        { SoundType.Cancel, Cancel },
        { SoundType.Start, Start },
        { SoundType.Finish, Finish },
        };
        }
    }
}
