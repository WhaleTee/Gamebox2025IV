using System;
using UnityEngine;

namespace Puzzle
{
    public class PieceEvents
    {
        public event Action<IPuzzlePiece> Activated;
        public event Action<Vector2, Vector3> Impact;
        public Action<PieceState> StateChanged;

        public void ChangeState(PieceState state) => StateChanged?.Invoke(state);
        public void TriggerImpact(Vector2 point, Vector3 normal) => Impact?.Invoke(point, normal);
        public void Activate(IPuzzlePiece piece) => Activated?.Invoke(piece);
    }
}