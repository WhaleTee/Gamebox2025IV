using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Rule/AnyOrder")]
    public class AnyOrderRuleConfig : PuzzleRuleConfig
    {
        public override IPuzzleRule CreateRule(List<IPuzzlePiece> pieces) => new AnyOrderRule(pieces);
    }
}
