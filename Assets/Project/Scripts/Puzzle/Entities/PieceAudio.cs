using Audio;
using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Entities
{
    [Serializable]
    public class PieceAudio
    {
        [SerializeField] private int m_polyphonyLimit = 4;
        [SerializeField] private List<AudioClip> m_impactSound;
        [SerializeField] private AudioClip m_deathSound;
        [SerializeField] private AudioClip m_activationSound;

        private AudioSource deathAudio;

        private List<AudioSource> impactAudios;
        private int currentImpactAudio;

        [Inject]
        private void Inject(AudioInjectionData audioData)
        {
            deathAudio = audioData.Get();

            impactAudios = new();
            for (int i = 0; i < m_polyphonyLimit; i++)
            {
                var impactAudio = audioData.Get();
                impactAudios.Add(audioData.Get());
            }
        }
    }
}