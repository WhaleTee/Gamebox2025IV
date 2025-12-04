using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Combat.Weapon
{
    [CreateAssetMenu(fileName = "WeaponAudioConfig_01", menuName = "Scriptables/Combat/Weapon/Effects/Audio")]
    public class WeaponConfigAudio : AudioConfigBase
    {
        [field: SerializeField] public int PolyphonyLimit { get; protected set; } = 3;
        [field: SerializeField] public AudioClip Shot { get; protected set; }
        [field: SerializeField] public AudioClip Exhausted { get; protected set; }
        [field: SerializeField] public AudioClip Cancel { get; protected set; }
        [field: SerializeField] public AudioClip Start { get; protected set; }
        [field: SerializeField] public AudioClip Finish { get; protected set; }
        public Dictionary<SoundType, AudioClip> Sounds { get; protected set; }

        public void OnEnable()
        {
            mixerGroup = Sound.SoundType.Combat;
            Sounds = new()
            {
                { SoundType.Shot, Shot },
                { SoundType.Exhausted, Exhausted },
                { SoundType.Cancel, Cancel },
                { SoundType.Start, Start },
                { SoundType.Finish, Finish },
            };
        }

        protected override void InitPrefab() => InitPrefab("WeaponAudio");
    }
}
