using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public abstract class PuzzleRuleConfig : ScriptableObject
    {
        [field: SerializeField] public bool DisablesOnSolve { get; private set; } = true;
        [field: SerializeField] public bool ResetOnFail { get; private set; }
        [field: SerializeField] public bool UseTimer { get; private set; } = false;
        [field: SerializeField] public float TimeLimit { get; private set; } = 10f;
        [field: SerializeField] public bool PieceCanBeDeactivated { get; private set; }
        [field: SerializeField] public int MaxAtempts { get; private set; } = 2;
        [field: SerializeField] public bool IsAttemptsPerPiece { get; private set; }
        [field: SerializeField] public float ResetInterval { get; private set; } = 3f;
        public abstract IPuzzleRule CreateRule(List<IPuzzlePiece> pieces);
    }
}