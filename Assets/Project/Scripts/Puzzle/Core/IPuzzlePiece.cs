using System;

namespace Puzzle
{
    public interface IPuzzlePiece
    {
        PieceEvents Events { get; }
        PieceState State { get; }

        void Install(PuzzleController puzzleController);
        void Activate();
    }
}
