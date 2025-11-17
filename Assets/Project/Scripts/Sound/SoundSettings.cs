using Misc;

namespace Sound
{
    public class SoundSettings : Singleton<SoundSettings>
    {
        private float musicVolume = 1;
        private float sfxVolume = 1;
        private float voiceVolume = 1;

        public float GetVolume(SoundType type)
        {
            return type switch
            {
                SoundType.Music => musicVolume,
                SoundType.SFX => sfxVolume,
                SoundType.Voice => voiceVolume,
                _ => 0
            };
        }
    }
}