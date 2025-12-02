using Cysharp.Threading.Tasks;
using Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        public PuzzleEvents Events { get; private set; }

        [SerializeField] private List<GameObject> m_pieces = new();
        private List<IPuzzlePiece> pieces;

        [SerializeField] private PuzzleRuleConfig m_ruleConfig;
        [SerializeField] private PuzzleAudio m_audio;

        private IPuzzleRule rule;

        private Dictionary<IPuzzlePiece, int> failedAttempts;
        private int totalFailedAttempts;
        private float timer;

        private bool isEnabled = true;

        private void OnValidate()
        {
            m_pieces.RemoveAll(go => go == null || !go.TryGetComponent<IPuzzlePiece>(out _));
        }

        private void Awake() => Install();
        private void Start() => Init();

        private void Install()
        {
            Events = new();
            pieces = new();
            failedAttempts = new();
            foreach (var piece in m_pieces)
            {
                var ipiece = piece.GetComponent<IPuzzlePiece>();
                ipiece.Install(this);
                pieces.Add(ipiece);
                failedAttempts.Add(ipiece, 0);
            }
            m_audio.InjectAttributes();
            m_audio.Install(Events);
        }

        private void Init()
        {

            rule = m_ruleConfig.CreateRule(pieces);
            rule.Init(pieces);

            timer = m_ruleConfig.TimeLimit;

            foreach (var piece in pieces)
                piece.Events.Activated += OnPieceActivated;

            m_audio.Init();
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

            if (!m_ruleConfig.PieceCanBeDeactivated || (piece.State & PieceState.Charged) == 0)
                ActivatePiece(piece);
            else
                DeactivatePiece(piece);
        }

        private void ActivatePiece(IPuzzlePiece piece)
        {
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
                    Fail();
                }
            }
        }

        private void DeactivatePiece(IPuzzlePiece piece)
        {
            rule.OnPieceDeactivated(piece);
            Events.OnDeactivated(piece);
        }

        private void Success()
        {
            if (m_ruleConfig.DisablesOnSolve)
                isEnabled = false;

            Events.OnSolve();
        }

        private void Fail()
        {
            if (m_ruleConfig.DisablesOnSolve)
                isEnabled = false;

            Events.OnFailed();
            if (m_ruleConfig.ResetInterval <= 0)
                ResetPuzzle();
            else
                UniTask.Action(m_ruleConfig.ResetInterval, ResetDelayed).Invoke();
        }

        private async UniTaskVoid ResetDelayed(float delay)
        {
            await UniTask.Delay(Mathf.RoundToInt(delay * 1000));
            ResetPuzzle();
        }

        private void ResetPuzzle()
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
