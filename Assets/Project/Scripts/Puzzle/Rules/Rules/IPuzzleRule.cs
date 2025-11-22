using System.Collections.Generic;

namespace Puzzle
{
    public interface IPuzzleRule
    {
        bool AllSolved { get; }
        void Init(List<IPuzzlePiece> pieces);
        bool OnPieceActivated(IPuzzlePiece piece);
        void Reset();
    }
}