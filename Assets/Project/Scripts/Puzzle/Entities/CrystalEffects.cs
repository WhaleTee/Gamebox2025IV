using Combat;
using Extensions;
using Puzzle.Entities;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Puzzle
{
    [Serializable]
    public class CrystallEffects
    {
        [SerializeField] private PieceAnimation m_pieceAnimation;
        [SerializeField] private PieceAudio m_pieceAudio;
        [SerializeField] private PieceParticles m_pieceParticles;
        [SerializeField] private Light2D m_light;

        private Crystal crystal;
        private string tweenID;

        public void OnValidate(GameObject gameObject)
        {
            m_pieceAnimation.OnValidate(gameObject);
            if (m_light == null)
                m_light = gameObject.GetComponent<Light2D>();
        }

        public void Install(Crystal crystal, PuzzleEvents events)
        {
            this.crystal = crystal;
            SetActive(true);

            tweenID = $"{nameof(CrystallEffects)}_{crystal.gameObject.name}";

            m_pieceAudio.InjectAttributes();
            m_pieceParticles.InjectAttributes();
            m_pieceParticles.Install(crystal.gameObject);
            m_pieceAnimation.Install(crystal.gameObject, tweenID);
        }

        public void SetActive(bool value)
        {
            if (value)
                SubAll();
            else
                UnsubAll();
        }

        private void StateChanged(PuzzleState state)
        {

        }

        private void OnReset()
        {

        }

        private void PlayCharged()
        {

        }

        private void FailedAttempt()
        {
        }

        private void PlayDeath()
        {
        }


        private void SubAll()
        {
            crystal.OnStateChanged += StateChanged;
        }

        private void UnsubAll()
        {
            crystal.OnStateChanged -= StateChanged;
        }
    }
}
