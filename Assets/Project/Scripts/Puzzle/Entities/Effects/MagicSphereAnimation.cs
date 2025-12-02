using UnityEngine;

namespace Puzzle.Entities
{
    public class MagicSphereAnimation : PieceAnimation<MagicSphereAnimationConfig>
    {
        protected override Sprite SetSprite(PieceState state) =>
            state switch
            {
                _ when (state & PieceState.Broken) != 0 => m_config.Broken,
                _ when (state & PieceState.Charged) != 0 => m_config.Activated,
                _ => m_config.NotActive
            };
    }
}