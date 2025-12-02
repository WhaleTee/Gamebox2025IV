using System;
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

        public IPuzzlePiece[] OnPieceDeactivated(IPuzzlePiece piece)
        {
            var index = allPieces.IndexOf(piece);
            var removed = new IPuzzlePiece[activated.Count - index];

            for (int i = index; i < activated.Count; i++)
            {
                removed[i] = allPieces[i];
                activated.Remove(allPieces[i]);
            }

            return removed;
        }
    }
}