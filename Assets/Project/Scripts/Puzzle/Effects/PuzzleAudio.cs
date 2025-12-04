using System;
using UnityEngine;
using Reflex.Attributes;
using Sound;

namespace Puzzle
{
    [Serializable]
    public class PuzzleAudio
    {
        [SerializeField] private PuzzleAudioConfig m_config;
        private PuzzleEvents events;

        private AudioSource solved;
        private AudioSource failed;
        private AudioSource reset;

        private Func<AudioSource, AudioSource> get;
        private Action<AudioSource> release;

        public void Install(PuzzleEvents events)
        {
            this.events = events;
        }

        public void Init()
        {
            Populate();
            SubAll();
        }

        private void Failed() { if (failed != null) failed.Play(); }

        private void Solved() { if (solved != null) solved.Play(); }

        private void Reset() { if (reset != null) reset.Play(); }

        public void SubAll()
        {
            events.Solved += Solved;
            events.Failed += Failed;
            events.Reset += Reset;
        }

        public void UnsubAll()
        {
            events.Solved -= Solved;
            events.Failed -= Failed;
            events.Reset -= Reset;
        }

        private void Populate()
        {
            solved = Get(m_config.Solved);
            failed = Get(m_config.Failed);
            reset = Get(m_config.Reset);
        }

        private AudioSource Get(AudioClip clip)
        {
            var source = get(m_config.SourcePrefab);
            source.clip = clip;
            return source;
        }

        [Inject]
        private void Inject(AudioInjectionData di)
        {
            get = di.Get;
            release = di.Release;
        }
    }
}