using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        public PuzzleEvents Events { get; private set; }

        [SerializeField] private List<GameObject> m_pieces;
        private List<IPuzzlePiece> pieces;

        [SerializeField] private PuzzleRuleConfig m_ruleConfig;
        [SerializeField] private GameObject m_mechanism;

        private IPuzzleRule rule;

        private Dictionary<IPuzzlePiece, int> failedAttempts;
        private int totalFailedAttempts;
        private float timer;

        private bool isEnabled = true;

        private void OnValidate()
        {
            for (int i = m_pieces.Count - 1; i >= 0; i--)
                if (!m_pieces[i].TryGetComponent(out IPuzzlePiece piece))
                    m_pieces.RemoveAt(i);
        }

        private void Awake() => Init();

        private void Init()
        {
            pieces = new();
            foreach (var piece in m_pieces)
            {
                var ipiece = piece.GetComponent<IPuzzlePiece>();
                pieces.Add(ipiece);
                failedAttempts.Add(ipiece, 0);
            }
        }

        private void Start()
        {
            rule = m_ruleConfig.CreateRule(pieces);
            rule.Init(pieces);

            timer = m_ruleConfig.TimeLimit;

            foreach (var piece in pieces)
                piece.OnActivate += OnPieceActivated;
        }

        private void Update()
        {
            if (!isEnabled || !m_ruleConfig.UseTimer)
                return;

            timer -= Time.deltaTime;

            if (timer <= 0)
                Fail();
        }

        private void OnPieceActivated(IPuzzlePiece piece)
        {
            if (!isEnabled)
                return;

            if (rule.OnPieceActivated(piece))
            {
                Events.OnCharged(piece);
                if (rule.AllSolved)
                    Success();
            }
            else
            {
                failedAttempts[piece]++;
                totalFailedAttempts++;

                Events.OnFailedAttempt(piece);

                if (m_ruleConfig.IsAttemptsPerPiece)
                    if (failedAttempts[piece] < m_ruleConfig.MaxAtempts)
                        return;
                if (!m_ruleConfig.IsAttemptsPerPiece)
                    if (totalFailedAttempts < m_ruleConfig.MaxAtempts)
                        return;

                if (m_ruleConfig.ResetOnFail)
                {
                    Events.OnDestroyed(piece);
                    Reset();
                }
            }
        }

        private void Success()
        {
            isEnabled = false;
            Events.OnSolve();
        }

        private void Fail()
        {
            isEnabled = false;
            Events.OnFailed();
        }

        private void Reset()
        {
            rule.Reset();
            timer = m_ruleConfig.TimeLimit;
            isEnabled = true;
            totalFailedAttempts = 0;

            for (int i = 0; i < failedAttempts.Count; i++)
                failedAttempts[pieces[i]] = 0;

            Events.OnReset();
        }
    }
}
