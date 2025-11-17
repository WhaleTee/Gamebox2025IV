using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public struct AudioWithType<T> where T : Enum
    {
        public T type;
        public SoundType soundType;
        public AudioClip[] audio;
    }
}