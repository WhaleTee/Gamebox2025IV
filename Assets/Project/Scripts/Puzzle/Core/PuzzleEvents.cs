using System;

namespace Puzzle
{
    public class PuzzleEvents
    {
        public event Action Reset;
        public event Action Solved;
        public event Action Failed;
        public event Action<IPuzzlePiece> Charged;
        public event Action<IPuzzlePiece> FailedAttempt;
        public event Action<IPuzzlePiece> Destroyed;

        public void OnReset() => Reset?.Invoke();
        public void OnSolve() => Solved?.Invoke();
        public void OnFailed() => Failed?.Invoke();
        public void OnCharged(IPuzzlePiece piece) => Charged?.Invoke(piece);
        public void OnFailedAttempt(IPuzzlePiece piece) => FailedAttempt?.Invoke(piece);
        public void OnDestroyed(IPuzzlePiece piece) => Destroyed?.Invoke(piece);
    }
}