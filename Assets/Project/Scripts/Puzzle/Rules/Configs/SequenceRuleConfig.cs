using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Rule/SequenceOrder")]
    public class SequenceRuleConfig : PuzzleRuleConfig
    {
        public override IPuzzleRule CreateRule(List<IPuzzlePiece> pieces) => new SequenceOrderRule(pieces);
    }
}
