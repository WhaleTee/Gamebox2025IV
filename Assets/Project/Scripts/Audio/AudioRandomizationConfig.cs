using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Effects/Audio/AudioRandomization")]
    public class AudioRandomizationConfig : ScriptableObject
    {
        [field: SerializeField] public Volume Volume { get; private set; }
        [field: SerializeField] public Pitch Pitch { get; private set; }
        [field: SerializeField] public PanStereo PanStereo { get; private set; }
    }

    [Serializable]
    public struct Volume
    {
        public float Min, Max;

        public Volume(float min = 0.9f, float max = 1.0f)
        {
            Min = min; Max = max;
        }
    }

    [Serializable]
    public struct Pitch
    {
        public float Min, Max;

        public Pitch(float min = 0.9f, float max = 1.05f)
        {
            Min = min; Max = max;
        }
    }

    [Serializable]
    public struct PanStereo
    {
        public float Min, Max;

        public PanStereo(float min = -0.1f, float max = 0.1f)
        {
            Min = min; Max = max;
        }
    }

    public static class AudioSourceExtensions
    {
        public static AudioSource PitchRand(this AudioSource source, AudioRandomizationConfig cfg)
        {
            var pitch = cfg.Pitch;
            source.pitch = Random.Range(pitch.Min, pitch.Max);
            return source;
        }

        public static AudioSource VolumeRand(this AudioSource source, AudioRandomizationConfig cfg)
        {
            var volume = cfg.Volume;
            source.volume = Random.Range(volume.Min, volume.Max);
            return source;
        }

        public static AudioSource PanRand(this AudioSource source, AudioRandomizationConfig cfg)
        {
            var pan = cfg.PanStereo;
            source.panStereo = Random.Range(pan.Min, pan.Max);
            return source;
        }

        public static AudioSource ClearRand(this AudioSource source)
        {
            source.pitch = 1;
            source.volume = 1;
            source.panStereo = 0;
            return source;
        }
    }
}
