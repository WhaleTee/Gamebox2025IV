using System;

namespace Puzzle
{
    public interface IPuzzlePiece
    {
        event Action<IPuzzlePiece> OnActivate;

        public void Install(PuzzleController puzzleController);
        public void Activate();
    }
}
