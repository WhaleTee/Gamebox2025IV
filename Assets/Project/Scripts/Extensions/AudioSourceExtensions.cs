using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Extensions
{
    public static class AudioSourceExtensions
    {
        private const float epsilon = 0.0001f; //float.Epsilon;

        public static float GetDuration(this AudioSource source)
        {
            if (!source.IsClipValid())
                return 0f;

            return source.clip.length / source.GetPitchModifier();
        }

        public static float GetRemainingDuration(this AudioSource source)
        {
            if (!source.IsClipValid())
                return 0f;

            float total = source.GetDuration();
            float played = source.time / source.GetPitchModifier();
            return Mathf.Max(0, total - played);
        }

        public static async void OnFinished(this AudioSource source, Action callback, CancellationToken cancellationToken = default)
        {
            await source.WaitUntilFinished(cancellationToken);
            callback?.Invoke();
        }

        public static async UniTask WaitUntilFinished(this AudioSource source, CancellationToken cancellationToken = default)
        {
            if (!source.IsPlayingValid())
                return;

            try { await UniTask.WaitUntil(() => !source.isPlaying, cancellationToken: cancellationToken); }
            catch(OperationCanceledException) { }
        }

        private static float GetPitchModifier(this AudioSource source) => Mathf.Max(source.pitch, epsilon);
        public static bool IsClipValid(this AudioSource source) => source != null && source.clip != null;
        public static bool IsPlayingValid(this AudioSource source) => source.IsClipValid() && source.isPlaying;
    }
}
