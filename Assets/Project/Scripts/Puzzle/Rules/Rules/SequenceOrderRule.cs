using System.Collections.Generic;

namespace Puzzle
{
    public class SequenceOrderRule : IPuzzleRule
    {
        public bool AllSolved => activated.SetEquals(sequence);
        private HashSet<IPuzzlePiece> activated;
        private List<IPuzzlePiece> sequence;
        private int index;

        public SequenceOrderRule(List<IPuzzlePiece> sequence)
        {
            this.sequence = sequence;
        }

        public void Init(List<IPuzzlePiece> pieces)
        {
        }

        public bool OnPieceActivated(IPuzzlePiece piece)
        {
            if (sequence[index] == piece)
            {
                index++;
                activated.Add(piece);
                return true;
            }
            else return false;
        }

        public void Reset()
        {
            index = 0;
            activated.Clear();
        }
    }
}