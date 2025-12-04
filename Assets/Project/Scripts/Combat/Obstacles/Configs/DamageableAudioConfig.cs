using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Combat
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Obstacles/Effects/Audio")]
    public class DamageableAudioConfig : AudioConfigBase
    {
        [field: SerializeField] public int PolyphonyLimit { get; private set; } = 4;
        [field: SerializeField] public List<AudioClip> ImpactSound { get; private set; }
        [field: SerializeField] public AudioClip DeathSound { get; private set; }

        private void OnEnable()
        {
            mixerGroup = Sound.SoundType.Combat;
        }

        protected override void InitPrefab() => InitPrefab("DamageableAudio");
    }
}
