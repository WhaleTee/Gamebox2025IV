using UnityEngine;
using Combat;
using Combat.Projectiles;

namespace Puzzle
{
    public class Crystal : MonoBehaviour, IPuzzlePiece, IDamageable
    {
        public PuzzlePieceAnimationConfig Config => m_effects.Config;
        [field: SerializeField] public PieceState InitialState { get; private set; }
        public PieceState State { get; private set; }
        public Vector2 LastImpactPoint { get; private set; }
        public Vector3 LastImpactNormal { get; private set; }

        public PieceEvents Events { get; private set; }

        [SerializeField] private DamageType m_takesDamageType;
        [SerializeField] private CrystallEffects m_effects;
        [SerializeField] private Misc.Collider.Collider2DListener trigger;

        private void OnValidate()
        {
            m_effects.OnValidate(gameObject);
        }

        public void Install(PuzzleController puzzleController)
        {
            SetState(InitialState);
            m_effects.Install(this, puzzleController.Events, Events);
            puzzleController.Events.Reset += ResetState;
            puzzleController.Events.Charge += Charge;
            puzzleController.Events.FailAttempt += FailedAttempt;
            puzzleController.Events.Broke += Broke;
            trigger.OnEnter += CollisionEnter;
        }

        public void Activate() => Events.Activate(this);

        public void SetState(PieceState state)
        {
            if (this.State == state)
                return;

            this.State = state;
            Events.ChangeState(State);
        }

        public void InflictDamage(Damage damage)
        {

        }

        private void ResetState() => SetState(InitialState);

        private void CollisionEnter(Collision2D collision)
        {
            bool hit = collision.gameObject.CompareTag("Projectile");
            if (!hit)
                return;

            bool canDealDamage = collision.gameObject.GetComponent<Projectile>().WeaponProjectileStats.Damage.Type.Has(m_takesDamageType);
            if (!canDealDamage)
                return;

            var contact = collision.contacts[0];
            var hitPoint = contact.point;
            var normal = contact.normal;

            LastImpactPoint = hitPoint;
            LastImpactNormal = normal;
            Events.TriggerImpact(hitPoint, normal);
            Activate();
        }

        private (Vector2 point, Vector3 normal) GetContact(Collider2D collider)
        {
            Vector2 origin = transform.position;
            Vector2 hitPoint = collider.ClosestPoint(origin);

            Vector2 dir = origin - hitPoint;
            Vector2 normal = dir.sqrMagnitude > 1e-6f ? dir.normalized : Vector2.up;

            return (hitPoint, normal);
        }

        private void FailedAttempt(IPuzzlePiece piece)
        {
            if ((IPuzzlePiece)this != piece)
                return;

            PieceState state = State;

            if ((state & PieceState.Damaged) == 0)
                state |= PieceState.Damaged;
            else if ((state & PieceState.DamagedAgain) == 0)
                state |= PieceState.DamagedAgain;

            SetState(state);
            m_effects.PlayFailedAttempt(piece);
        }

        private void Deactivate(IPuzzlePiece piece)
        {
            if ((IPuzzlePiece)this != piece)
                return;

            SetState(State.Remove(PieceState.Charged));
            m_effects.PlayDeactivate(piece);
        }

        private void Charge(IPuzzlePiece piece)
        {
            if ((IPuzzlePiece)this != piece)
                return;

            SetState(State.Add(PieceState.Charged));
            m_effects.PlayCharge(piece);
        }

        private void Discharge(IPuzzlePiece piece)
        {
            if ((IPuzzlePiece)this != piece)
                return;

            SetState(State.Remove(PieceState.Charged));
            m_effects.PlayDischarge(piece);
        }

        private void Broke(IPuzzlePiece piece)
        {
            if ((IPuzzlePiece)this != piece)
                return;

            SetState(PieceState.Broken);
            m_effects.PlayBroke(piece);
        }
    }
}