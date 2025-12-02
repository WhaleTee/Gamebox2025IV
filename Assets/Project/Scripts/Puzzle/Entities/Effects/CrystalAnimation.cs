using UnityEngine;
using Puzzle.Entities;

namespace Puzzle
{
    public class CrystalAnimation : PieceAnimation<CrystalAnimationConfig>
    {
        public override void PlayImpact(Vector2 point, Vector3 normal, bool strong)
        {
            base.PlayImpact(point, normal, strong);
        }

        protected override Sprite SetSprite(PieceState state) =>
            state switch
            {
                _ when (state & PieceState.Broken) != 0 => m_config.Broken,
                _ when (state & PieceState.Charged) != 0 && (state & PieceState.DamagedAgain) != 0 => m_config.ChargedStronglyDamaged,
                _ when (state & PieceState.Charged) != 0 && (state & PieceState.Damaged) != 0 => m_config.ChargedDamaged,
                _ when (state & PieceState.Charged) != 0 => m_config.Charged,
                _ when (state & PieceState.DamagedAgain) != 0 => m_config.StronglyDamaged,
                _ when (state & PieceState.Damaged) != 0 => m_config.Damaged,
                _ => m_config.Intact
            };
    }
}