using System;

namespace Puzzle
{
    [Flags]
    public enum PuzzleState
    {
        Charged = 1 >> 0,
        Broken = 1 >> 1,
        Damaged = 1 >> 2,
        DamagedAgain = 1 >> 3,
    }
}