using System;
using UnityEngine;
using MovementConfig = Movement.PresetObject;

namespace LevelProgression
{
    public class PlayerAbilities : MonoBehaviour
    {
        public Func<DamageBundle> DamageBundle;
        public int Jumps => jumps.CalculatedValue;
        public float MovementSpeed => movementSpeed.CalculatedValue;

        [SerializeField] private MovementConfig movementConfig;
        private Stat<DamageBundle> damageBundle;
        private Stat<int> jumps;
        private Stat<float> movementSpeed;

        private void Awake() => Install();

        public void Install()
        {
            damageBundle = new(new());
            DamageBundle = GetDamage;
            jumps = new(movementConfig.AirMovementSettings.maxAirJumps + 1);
            movementSpeed = new(movementConfig.GroundMovementSettings.maxSpeed);
        }

        public void SetBaseDamage(DamageBundle value) => damageBundle.SetBase(value);

        public void AddModifier(DamageModifier mod) => damageBundle.Add(mod);
        public void AddModifier(JumpModifier mod) => jumps.Add(mod);
        public void AddModifier(MovementSpeedModifier mod) => movementSpeed.Add(mod);
        public void RemoveModifier(DamageModifier mod) => damageBundle.Remove(mod);
        public void RemoveModifier(JumpModifier mod) => jumps.Remove(mod);
        public void RemoveModifier(MovementSpeedModifier mod) => movementSpeed.Remove(mod);

        private DamageBundle GetDamage() => damageBundle.CalculatedValue;
    }
}