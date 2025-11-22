using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Rule/SequenceOrder")]
    public class SequenceRuleConfig : PuzzleRuleConfig
    {
        public List<IPuzzlePiece> Sequence { get; private set; }
        [SerializeField] private List<GameObject> m_sequence;

        public override IPuzzleRule CreateRule(List<IPuzzlePiece> pieces) => new SequenceOrderRule(Sequence);

        private void OnEnable()
        {
            for (int i = 0; i < m_sequence.Count; i++)
                Sequence.Add(m_sequence[i].GetComponent<IPuzzlePiece>());
        }
    }
}
