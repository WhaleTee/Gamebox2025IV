using System;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Sound
{
    [Serializable]
    public struct SoundDataContainer
    {
        public SoundType Type;
        public AudioMixerGroup MixerGroup;
        public Slider Slider;
        public Toggle Toggle;
    }
}