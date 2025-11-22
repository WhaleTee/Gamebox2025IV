using System.Collections.Generic;

namespace Puzzle
{
    public class AnyOrderRule : IPuzzleRule
    {
        public bool AllSolved => activated.Count >= allPieces.Count && activated.IsSupersetOf(allPieces);
        private HashSet<IPuzzlePiece> activated = new();
        private List<IPuzzlePiece> allPieces;

        public AnyOrderRule(List<IPuzzlePiece> pieces)
        {
            allPieces = pieces;
        }

        public void Init(List<IPuzzlePiece> pieces) { }

        public bool OnPieceActivated(IPuzzlePiece piece)
        {
            activated.Add(piece);
            return true;
        }

        public void Reset()
        {
            activated.Clear();
        }
    }
}