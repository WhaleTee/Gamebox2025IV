using System;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using Combat.Projectiles;

namespace Puzzle
{
    public class Crystal : MonoBehaviour, IPuzzlePiece, IDamageable
    {
        [field: SerializeField] public PuzzleState InitialState { get; private set; }
        public event Action<IPuzzlePiece> OnActivate;
        public event Action<Vector2, Vector3> OnImpact;
        [SerializeField] private DamageType m_takesDamageType;
        public Action<PuzzleState> OnStateChanged;
        private PuzzleState state;

        [SerializeField] private CrystallEffects m_effects;

        private void OnValidate()
        {
            m_effects.OnValidate(gameObject);
        }

        public void Install(PuzzleController puzzleController)
        {
            SetState(InitialState);
            m_effects.Install(this, puzzleController.Events);
            puzzleController.Events.Reset += ResetState;
        }

        public void Activate() => OnActivate?.Invoke(this);

        public void SetState(PuzzleState state)
        {
            if (this.state == state)
                return;

            this.state = state;
            OnStateChanged?.Invoke(state);
        }

        public void InflictDamage(Damage damage)
        {

        }

        private void ResetState() => SetState(InitialState);

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool hit = collision.gameObject.CompareTag("Projectile");
            if (!hit)
                return;

            bool canDealDamage = collision.gameObject.GetComponent<Projectile>().WeaponProjectileStats.Damage.Type.Has(m_takesDamageType);
            if (!canDealDamage)
                return;

            var contact = collision.GetContact(0);
            OnImpact?.Invoke(contact.point, contact.normal);
            Activate();
        }
    }
}