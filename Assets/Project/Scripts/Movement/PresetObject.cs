using UnityEngine;

namespace Project.Scripts.Movement
{
    [CreateAssetMenu(menuName = "Character/Movement Preset", fileName = " - Character Movement Preset")]
    public class PresetObject : ScriptableObject
    {
        [field: SerializeField] public string PresetName { get; private set; }

        [field: SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How fast to reach max speed")]
        public float Acceleration { get; private set; }

        [field: SerializeField]
        [Range(0f, 20f)]
        [Tooltip("Maximum movement speed")]
        public float TopSpeed { get; private set; }

        [field: SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How fast to stop after letting go")]
        public float Deceleration { get; private set; }

        [field: SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How fast to stop when changing direction")]
        public float TurnSpeed { get; private set; }

        [field: SerializeField]
        [Range(2f, 5.5f)]
        [Tooltip("Maximum jump height")]
        public float JumpHeight { get; private set; }

        [field: SerializeField]
        [Range(0.2f, 1.25f)]
        [Tooltip("How long it takes to reach that height before coming back down")]
        public float TimeToApex { get; private set; }

        [field: SerializeField]
        [Range(1f, 10f)]
        [Tooltip("Gravity multiplier to apply when coming down")]
        public float DownwardMovementMultiplier { get; private set; }

        [field: SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How fast to reach max speed when in mid-air")]
        public float AirControl { get; private set; }

        [field: SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How fast to stop when changing direction when in mid-air")]
        public float AirControlActual { get; private set; }

        [field: SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How fast to stop in mid-air when no direction is used")]
        public float AirBrake { get; private set; }

        [field: SerializeField]
        [Tooltip("Should the character drop when you let go of jump?")]
        public bool VariableJumpHeight { get; private set; }

        [field: SerializeField]
        [Range(1f, 10f)]
        [Tooltip("Gravity multiplier when you let go of jump")]
        public float JumpCutoff { get; private set; }

        [field: SerializeField]
        [Range(0, 1)]
        [Tooltip("How many times can you jump in the air?")]
        public int AirJumps { get; private set; }
    }
}