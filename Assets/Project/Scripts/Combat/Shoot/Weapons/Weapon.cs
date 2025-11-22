using UnityEngine;
using Characters.Equipment;
using Extensions;
using Random = UnityEngine.Random;
using WeaponStateMachine = Combat.Weapon.State.StateMachine;
using Audio;

namespace Combat.Weapon
{
    public partial class Weapon<TStats> : Equipment<WeaponConfig<TStats>, TStats>, IUsableEquipment where TStats : WeaponStats
    {
        [field: SerializeField] public Transform FirePoint { get; protected set; }
        public Events Events { get; protected set; }
        protected WeaponStateMachine stateMachine;
        protected Blackboard blackboard;
        protected Effects effects;

        public virtual void Install()
        {
            Events = new(FirePoint);
            stateMachine = new();
            effects = new();
            blackboard = new();
            blackboard.Mana = 1;
            stateMachine.Install(Config.Stats, blackboard, Events);
            effects.Install(transform, Events, FirePoint, Config.Audio, Config.VisualEffects);
            gameObject.InjectGameObject();
        }

        public virtual void Init()
        {
            effects.Init();
            stateMachine.Init();
            Events.OnShot += Shot;
        }

        public override void SetActive(bool value)
        {
            base.SetActive(value);
            blackboard.Selected = value;
        }

        public void Tick() => stateMachine.Tick();

        protected void OnDisable() => stateMachine.OnDisable();

        protected virtual void Shot(Vector2 origin) {}

        protected virtual void Update()
        {
            if (blackboard.Selected)
                Tick();
        }

        protected Ray GetShotRay(Vector3 point, Vector3 direction,  WeaponStats stats)
        {
            var origin = point;
            float spreadOffset = stats.SpreadOffset;
            float spreadAngle = stats.SpreadAngle;

            if (spreadOffset > 0)
                origin += Random.insideUnitSphere * spreadOffset;

            if (spreadAngle > 0)
            {
                Quaternion spreadRotation = Quaternion.Euler(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0f);
                direction = spreadRotation * direction;
            }

            return new(origin, direction);
        }

        protected Ray2D GetShotRay(Vector2 point, Vector2 direction, WeaponStats stats)
        {
            var origin = point;
            float spreadOffset = stats.SpreadOffset;
            float spreadAngle = stats.SpreadAngle;

            if (spreadOffset > 0)
                origin += Random.insideUnitCircle * spreadOffset;

            if (spreadAngle > 0)
            {
                Quaternion spreadRotation = Quaternion.Euler(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0f);
                    direction = spreadRotation * direction;
            }

            return new(origin, direction);
        }

        void IUsableEquipment.UseStart() => stateMachine.Fire();
        void IUsableEquipment.UseCancel() => stateMachine.FireCancel();
    }
}