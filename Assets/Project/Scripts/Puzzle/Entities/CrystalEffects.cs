using System;
using UnityEngine;
using Extensions;
using Puzzle.Entities;

namespace Puzzle
{
    [Serializable]
    public class CrystallEffects
    {
        public PuzzlePieceAnimationConfig Config => m_pieceAnimation.Config;
        [SerializeField] private CrystalAnimation m_pieceAnimation;
        [SerializeField] private PieceAudio m_pieceAudio;
        [SerializeField] private PieceParticles m_pieceParticles;

        private Crystal crystal;
        private PuzzleEvents events;
        private string tweenID;

        public void OnValidate(GameObject gameObject)
        {
            m_pieceAnimation.OnValidate(gameObject);
        }

        public void Install(Crystal crystal, PuzzleEvents events, PieceEvents selfEvents)
        {
            this.crystal = crystal;
            this.events = events;
            SetActive(true);

            tweenID = $"{nameof(CrystallEffects)}_{crystal.gameObject.name}";

            m_pieceAudio.InjectAttributes();
            m_pieceParticles.InjectAttributes();
            m_pieceAudio.Install(crystal.transform);
            m_pieceParticles.Install(crystal.gameObject);
            m_pieceAnimation.Install(crystal.gameObject, events, selfEvents, tweenID);
        }

        public void SetActive(bool value)
        {
            m_pieceAnimation.SetActive(value);
            m_pieceAudio.SetActive(value);
            m_pieceParticles.SetActive(value);
        }

        public void PlayCharge(IPuzzlePiece piece)
        {
            m_pieceAnimation.PlayCharge();
            m_pieceAudio.PlayCharge();
            m_pieceParticles.PlayCharge();
        }

        public void PlayDischarge(IPuzzlePiece piece)
        {
            m_pieceAnimation.PlayDischarge();
            m_pieceAudio.PlayDischarge();
            m_pieceParticles.PlayDischarge();
        }

        public void PlayDeactivate(IPuzzlePiece piece)
        {
            m_pieceAnimation.PlayDeactivate();
            m_pieceAudio.PlayDeactivate();
            m_pieceParticles.PlayDeactivate();
        }

        public void PlayFailedAttempt(IPuzzlePiece puzzlePiece)
        {
            if (!crystal.State.Has(PieceState.Damaged) && !crystal.State.Has(PieceState.DamagedAgain))
                return;

            bool stronglyDamaged = crystal.State.Has(PieceState.DamagedAgain);
            m_pieceAudio.PlayImpact(crystal.LastImpactPoint, crystal.LastImpactNormal, stronglyDamaged);
            m_pieceParticles.PlayImpact(crystal.LastImpactPoint, crystal.LastImpactNormal, stronglyDamaged);
            m_pieceAnimation.PlayImpact(crystal.LastImpactPoint, crystal.LastImpactNormal, stronglyDamaged);
        }

        public void PlayBroke(IPuzzlePiece puzzlePiece)
        {
            m_pieceAudio.PlayDeath();
            m_pieceParticles.PlayDeath();
        }
    }
}
