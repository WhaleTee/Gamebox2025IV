using System;
using UnityEngine;

namespace Puzzle.Entities
{
    public class PolyCrystalAnimation : PieceAnimation<PolyCrystalAnimationConfig>
    {
        protected override Sprite SetSprite(PieceState state) =>
            state switch
            {
                _ when (state & PieceState.Charged) != 0 => m_config.Activated,
                _ => m_config.NotActive
            };
    }
}
