using System;

namespace Puzzle
{
    public class PuzzleEvents
    {
        public event Action Reset;
        public event Action Solved;
        public event Action Failed;
        public event Action<IPuzzlePiece> Charge;
        public event Action<IPuzzlePiece> Discharge;
        public event Action<IPuzzlePiece> Deactivate;
        public event Action<IPuzzlePiece> FailAttempt;
        public event Action<IPuzzlePiece> Broke;

        public void OnReset() => Reset?.Invoke();
        public void OnSolve() => Solved?.Invoke();
        public void OnFailed() => Failed?.Invoke();
        public void OnCharged(IPuzzlePiece piece) => Charge?.Invoke(piece);
        public void OnDischarged(IPuzzlePiece piece) => Discharge?.Invoke(piece);
        public void OnDeactivated(IPuzzlePiece piece) => Deactivate?.Invoke(piece);
        public void OnFailedAttempt(IPuzzlePiece piece) => FailAttempt?.Invoke(piece);
        public void OnDestroyed(IPuzzlePiece piece) => Broke?.Invoke(piece);
    }
}